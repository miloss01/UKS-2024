﻿using DockerHubBackend.Dto.Request;
using DockerHubBackend.Dto.Response;
using DockerHubBackend.Models;
using DockerHubBackend.Repository.Interface;
using DockerHubBackend.Services.Implementation;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerHubBackend.Tests.UnitTests
{
    public class DockerImageServiceTests
    {
        private readonly Mock<IDockerImageRepository> _mockDockerImageRepository;
        private readonly DockerImageService _service;

        public DockerImageServiceTests()
        {
            _mockDockerImageRepository = new Mock<IDockerImageRepository>();
            _service = new DockerImageService(_mockDockerImageRepository.Object);
        }

        [Fact]
        public void GetDockerImages_SearchTermAndBadgesEmpty_ReturnsAllDockerImages()
        {
            var dockerImage1 = new DockerImage { Id = Guid.NewGuid(), DockerRepositoryId = Guid.NewGuid(), Repository = null };
            var dockerImage2 = new DockerImage { Id = Guid.NewGuid(), DockerRepositoryId = Guid.NewGuid(), Repository = null };

            var dockerImages = new List<DockerImage>();
            dockerImages.Add(dockerImage1);
            dockerImages.Add(dockerImage2);

            var pageDto = new PageDTO<DockerImage> { Data = dockerImages, TotalNumberOfElements = dockerImages.Count };

            var page = 1;
            var pageSize = 100;
            var searchTerm = "";
            var badges = "";

            _mockDockerImageRepository.Setup(dockerImageRepository => dockerImageRepository.GetDockerImages(page, pageSize, searchTerm, badges)).Returns(pageDto);

            var result = _service.GetDockerImages(page, pageSize, searchTerm, badges);

            Assert.Equal(2, result.Data.Count);
            Assert.Equal(dockerImages, result.Data);
        }

        [Fact]
        public void GetDockerImages_SearchTermNotEmpty_ReturnsDockerImagesWhichIdContainsSearchTerm()
        {
            var dockerImage1 = new DockerImage { Id = Guid.NewGuid(), DockerRepositoryId = Guid.NewGuid(), Repository = null };
            var dockerImage2 = new DockerImage { Id = Guid.NewGuid(), DockerRepositoryId = Guid.NewGuid(), Repository = null };

            var dockerImages = new List<DockerImage>();
            dockerImages.Add(dockerImage1);

            var pageDto = new PageDTO<DockerImage> { Data = dockerImages, TotalNumberOfElements = dockerImages.Count };

            var page = 1;
            var pageSize = 100;
            var searchTerm = dockerImage1.Id.ToString().Substring(0, 10);
            var badges = "";

            _mockDockerImageRepository.Setup(dockerImageRepository => dockerImageRepository.GetDockerImages(page, pageSize, searchTerm, badges)).Returns(pageDto);

            var result = _service.GetDockerImages(page, pageSize, searchTerm, badges);

            Assert.Equal(1, result.Data.Count);
            Assert.Equal(dockerImage1, result.Data.First());
        }

        [Fact]
        public void GetDockerImages_BadgesNotEmpty_ReturnsDockerImagesWhichRepositoryHasProvidedBadge()
        {
            var dockerImage1 = new DockerImage { Id = Guid.NewGuid(), DockerRepositoryId = Guid.NewGuid(), Repository = new DockerRepository { Name = "repo1", Badge = Badge.DockerOfficialImage } };
            var dockerImage2 = new DockerImage { Id = Guid.NewGuid(), DockerRepositoryId = Guid.NewGuid(), Repository = new DockerRepository { Name = "repo2", Badge = Badge.VerifiedPublisher } };

            var dockerImages = new List<DockerImage>();
            dockerImages.Add(dockerImage1);
            dockerImages.Add(dockerImage2);

            var pageDto = new PageDTO<DockerImage> { Data = dockerImages, TotalNumberOfElements = dockerImages.Count };

            var page = 1;
            var pageSize = 100;
            var searchTerm = "";
            var badges = "DockerOfficialImage,VerifiedPublisher";

            _mockDockerImageRepository.Setup(dockerImageRepository => dockerImageRepository.GetDockerImages(page, pageSize, searchTerm, badges)).Returns(pageDto);

            var result = _service.GetDockerImages(page, pageSize, searchTerm, badges);

            Assert.Equal(2, result.Data.Count);
            Assert.Equal(dockerImages, result.Data);
        }
    }
}
