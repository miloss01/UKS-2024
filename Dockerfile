FROM docker.elastic.co/elasticsearch/elasticsearch:8.6.0

ENV discovery.type=single-node
ENV xpack.security.enabled=false

# Disable SSL for HTTP
RUN echo "xpack.security.http.ssl.enabled: false" >> /usr/share/elasticsearch/config/elasticsearch.yml
RUN echo "xpack.security.transport.ssl.enabled: false" >> /usr/share/elasticsearch/config/elasticsearch.yml
