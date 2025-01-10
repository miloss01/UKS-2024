from uuid import uuid4
from datetime import datetime, timedelta, timezone
import logging
import base64
from typing import List

from flask import Flask, jsonify, request
import jwt
from cryptography.x509 import load_pem_x509_certificate
from cryptography.hazmat.backends import default_backend
from cryptography.hazmat.primitives import serialization

from scope import Scope
from access_service import AccessService
from config import TOKEN_VALID_FOR_SECONDS
from config import PRIVILEGED_USERS

app = Flask(__name__)
app.debug = True

app.config['SERVICE'] = 'uks-registry'  # eg. "registry.ceph.com"
app.config['ISSUER'] = 'uks.registry-auth'  # eg. "registry-auth.ceph.com"
access_service = AccessService()

# To generate a cert and key:
# openssl req -x509 -newkey rsa:4096 -keyout key.pem -out cert.pem -days 365 -nodes -subj '//SKIP=skip/CN=uks-registry'

def get_certificate():
    """Return a cryptography.x509.Certificate object"""
    with open('cert.pem', 'rb') as cert_file:
        return load_pem_x509_certificate(cert_file.read(),
                                         default_backend())

def get_private_key():
    with open('key.pem', 'rb') as key_file:
        private_key = serialization.load_pem_private_key(
            key_file.read(),
            password=None,
            backend=default_backend()
        )
        return private_key


def get_token_payload(service, issuer, scopes, user_id):
    now = datetime.now(timezone.utc)
    token_payload = {
                     'iss' : issuer,
                     'sub' : user_id,
                     'aud' : service,
                     'exp' : now + timedelta(seconds=TOKEN_VALID_FOR_SECONDS),
                     'nbf' : now,
                     'iat' : now,
                     'jti' : uuid4().hex,
                     'access' : [
                                {
                                 'type' : scope.type,
                                 'name' : scope.name,
                                 'actions' : scope.actions
                                 }
                                for scope in scopes
                                 ]
                     }
    return token_payload


def construct_token_response(service, issuer, scopes, user_id):
    token_payload = get_token_payload(service, issuer, scopes, user_id)
    cert = get_certificate()
    x5c = cert.public_bytes(serialization.Encoding.PEM)
    
    # Decode the bytes to a string and then perform replace
    x5c = x5c.decode('utf-8')  # Decode bytes to string
    x5c = x5c.replace('\n', '')  # Remove newlines
    x5c = x5c.replace('-----BEGIN CERTIFICATE-----', '')  # Remove certificate header
    x5c = x5c.replace('-----END CERTIFICATE-----', '')  # Remove certificate footer
    
    # Prepare the response payload
    response_payload = {
        'token': jwt.encode(
            token_payload,
            get_private_key(),
            headers={'x5c': [x5c]},
            algorithm='RS256'
        ),
        'expires_in': 3600,
        'issued_at': datetime.now(timezone.utc).isoformat()
    }

    return response_payload


def get_scopes():
    scopes = set()
    for s in request.args.getlist('scope'):
        scopes.add(Scope.parse(s))
    return scopes


def decode_basic_auth():
    auth_header = request.headers.get('Authorization')
    if auth_header is None or not auth_header.startswith('Basic '):
        return None
    
    # Extract the base64-encoded credentials part of the header
    encoded_credentials = auth_header.split(' ')[1]
    
    # Decode the base64 credentials
    decoded_credentials = base64.b64decode(encoded_credentials).decode('utf-8')
    
    # Split into username and password
    username, password = decoded_credentials.split(':', 1)
    
    return username, password

def authenticate():
    username, password = decode_basic_auth()
    if username == None or password == None:
        return None, None
    if username in PRIVILEGED_USERS and PRIVILEGED_USERS[username] == password:
        return username, True
    return access_service.authenticate_user(username, password), False

def authorize(user_id, scopes: List[Scope]):
    for scope in scopes:
        if scope.type == "repository":
            for action in scope.actions:
                if action == "pull" and not access_service.check_pull_permission(scope.name, user_id):
                    return False
                elif action == "push" and not access_service.check_push_permission(scope.name, user_id):
                    return False
    return True
                    

@app.route('/token')
def token():
    """
    See https://github.com/docker/distribution/blob/master/docs/spec/auth/token.md#requesting-a-token
    """
        # Print the full HTTP request method and headers
    print(f"Method: {request.method}")
    print(f"URL: {request.url}")
    print(f"Headers: {request.headers}")
    
    # Print the raw body of the request (if any)
    if request.data:
        print(f"Body: {request.data.decode('utf-8')}")
    else:
        print("Body: None")

    try:
        service = request.args['service']
    except KeyError:
        response = jsonify({'error': 'service parameter required'})
        response.status_code = 400
        return response
    if service != app.config['SERVICE']:
        response = jsonify({'error': 'service parameter incorrect'})
        response.status_code = 400
        return response
    scopes = get_scopes()
    
    user_id, is_privileged = authenticate()

    if not user_id:
        response = jsonify({'error': 'authentication unsuccessfull'})
        response.status_code = 401
        return response
    
    if not is_privileged:
        if not authorize(user_id, scopes):
            response = jsonify({'error': 'insufficient permissions'})
            response.status_code = 401
            return response
    # scopes.add(Scope("registry", "catalog", None, ["*"]))
    # scopes.add(Scope("repository","*", None, ["*"]))
    payload = construct_token_response(service, app.config['ISSUER'], scopes, user_id)
    return jsonify(**payload)


app.logger.setLevel(logging.DEBUG)
if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5001, debug=True)