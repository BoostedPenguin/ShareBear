using AutoMapper;
using ByteSizeLib;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Moq;
using ShareBear.Data;
using ShareBear.Data.Models;
using ShareBear.Profiles;
using ShareBear.Services;
using System.Security.Claims;
using System.Text;

namespace ShareBear.UnitTests
{
    public class FileServiceTests
    {
        [Fact]
        public async Task GetContainerFilesTest()
        {
            // Arrange
            var content = "Hello World from a Fake File";
            var fileName = "test.pdf";
            var signedItemUrl = "azuretesturl.com";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);
            var mockContextFactory = new TestDbContextFactory();

            var azureMock = new Mock<IAzureStorageService>();

            var envMock = Mock.Of<IWebHostEnvironment>(h => h.EnvironmentName == "Development");


            azureMock.Setup(azureService => azureService.GetTotalSizeContainers().Result).Returns(ByteSize.FromBytes(50));
            azureMock.Setup(azureService => azureService.GetSignedContainerDownloadLinks(It.IsAny<string>()).Result)
                .Returns(new ContainerSASItems[]
            {
                new ContainerSASItems(fileName, signedItemUrl)
            });


            var myProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);


            // Act
            var mock = new FileAccessService(mockContextFactory, azureMock.Object, null, mapper, envMock);

            var generateContainer = await mock.GenerateContainer("visitorId", new List<IFormFile>()
            {
                file
            });


            var getContainer = await mock.GetContainerFiles("visitorId", generateContainer.ShortCodeString, true);


            // Assert
            Assert.NotNull(getContainer);
            Assert.Single(getContainer.ContainerFiles);
            Assert.Equal("visitorId", getContainer.CreatedByVisitorId);
        }


        [Fact]
        public async Task GetContainerFilesExceptionTest()
        {
            // Arrange
            var content = "Hello World from a Fake File";
            var fileName = "test.pdf";
            var signedItemUrl = "azuretesturl.com";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);
            var mockContextFactory = new TestDbContextFactory();

            var azureMock = new Mock<IAzureStorageService>();

            var envMock = Mock.Of<IWebHostEnvironment>(h => h.EnvironmentName == "Development");


            azureMock.Setup(azureService => azureService.GetTotalSizeContainers().Result).Returns(ByteSize.FromBytes(50));
            azureMock.Setup(azureService => azureService.GetSignedContainerDownloadLinks(It.IsAny<string>()).Result)
                .Returns(new ContainerSASItems[]
            {
                new ContainerSASItems(fileName, signedItemUrl)
            });


            var myProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);


            // Act
            var mock = new FileAccessService(mockContextFactory, azureMock.Object, null, mapper, envMock);

            var generateContainer = await mock.GenerateContainer("nonVisitorId", new List<IFormFile>()
            {
                file
            });


            var getContainer = await mock.GetContainerFiles("nonVisitorId", generateContainer.ShortCodeString, true);


            // Assert
            Assert.NotNull(getContainer);
            Assert.Single(getContainer.ContainerFiles);
            Assert.NotEqual("visitorId", getContainer.CreatedByVisitorId);
        }

        [Fact]
        public async Task GenerateContainerTest()
        {
            // Arrange
            var content = "Hello World from a Fake File";
            var fileName = "test.pdf";
            var signedItemUrl = "azuretesturl.com";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);
            var mockContextFactory = new TestDbContextFactory();

            var azureMock = new Mock<IAzureStorageService>();

            var envMock = Mock.Of<IWebHostEnvironment>(h => h.EnvironmentName == "Development");


            azureMock.Setup(azureService => azureService.GetTotalSizeContainers().Result).Returns(ByteSize.FromBytes(50));
            azureMock.Setup(azureService => azureService.GetSignedContainerDownloadLinks(It.IsAny<string>()).Result)
                .Returns(new ContainerSASItems[]
            {
                new ContainerSASItems(fileName, signedItemUrl)
            });

            var myProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);


            // Act
            var mock = new FileAccessService(mockContextFactory, azureMock.Object, null, mapper, envMock);

            var result = await mock.GenerateContainer("visitorId", new List<IFormFile>()
            {
                file
            });

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.ContainerFiles);
            Assert.Equal("visitorId", result.CreatedByVisitorId);
        }

        [Fact]
        public async Task GenerateContainerExceptionTest()
        {
            // Arrange
            var content = "Hello World from a Fake File";
            var fileName = "test.pdf";
            var signedItemUrl = "azuretesturl.com";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);
            var mockContextFactory = new TestDbContextFactory();

            var azureMock = new Mock<IAzureStorageService>();

            var envMock = Mock.Of<IWebHostEnvironment>(h => h.EnvironmentName == "Development");


            azureMock.Setup(azureService => azureService.GetTotalSizeContainers().Result).Returns(ByteSize.FromBytes(50));
            azureMock.Setup(azureService => azureService.GetSignedContainerDownloadLinks(It.IsAny<string>()).Result)
                .Returns(new ContainerSASItems[]
            {
                new ContainerSASItems(fileName, signedItemUrl)
            });


            var myProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);


            // Act
            var mock = new FileAccessService(mockContextFactory, azureMock.Object, null, mapper, envMock);

            var result = await mock.GenerateContainer("nonVisitorId", new List<IFormFile>()
            {
                file
            });


            // Assert
            Assert.NotNull(result);
            Assert.Single(result.ContainerFiles);
            Assert.NotEqual("visitorId", result.CreatedByVisitorId);
        }


        [Fact]
        public async Task GetStorageStatisticsTest()
        {
            // Arrange
            var content = "Hello World from a Fake File";
            var fileName = "test.pdf";
            var signedItemUrl = "azuretesturl.com";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);
            var mockContextFactory = new TestDbContextFactory();

            var azureMock = new Mock<IAzureStorageService>();

            var envMock = Mock.Of<IWebHostEnvironment>(h => h.EnvironmentName == "Development");


            azureMock.Setup(azureService => azureService.GetTotalSizeContainers().Result).Returns(ByteSize.FromBytes(50));
            azureMock.Setup(azureService => azureService.GetSignedContainerDownloadLinks(It.IsAny<string>()).Result)
                .Returns(new ContainerSASItems[]
            {
                new ContainerSASItems(fileName, signedItemUrl)
            });


            var myProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);


            // Act
            var mock = new FileAccessService(mockContextFactory, azureMock.Object, null, mapper, envMock);

            var result = await mock.GetStorageStatistics();


            // Assert
            Assert.NotNull(result);
            Assert.Equal(50, result.UsedStorageBytes);
            Assert.True(result.HasFreeSpace);
        }

        [Fact]
        public async Task GetStorageStatisticsExceptionTest()
        {
            // Arrange
            var content = "Hello World from a Fake File";
            var fileName = "test.pdf";
            var signedItemUrl = "azuretesturl.com";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);
            var mockContextFactory = new TestDbContextFactory();

            var azureMock = new Mock<IAzureStorageService>();

            var envMock = Mock.Of<IWebHostEnvironment>(h => h.EnvironmentName == "Development");


            azureMock.Setup(azureService => azureService.GetTotalSizeContainers().Result).Returns(ByteSize.FromGigaBytes(1));
            azureMock.Setup(azureService => azureService.GetSignedContainerDownloadLinks(It.IsAny<string>()).Result)
                .Returns(new ContainerSASItems[]
            {
                new ContainerSASItems(fileName, signedItemUrl)
            });


            var myProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);


            // Act
            var mock = new FileAccessService(mockContextFactory, azureMock.Object, null, mapper, envMock);

            var result = await mock.GetStorageStatistics();


            // Assert
            Assert.NotNull(result);
            Assert.Equal(1000000000, result.UsedStorageBytes);
            Assert.False(result.HasFreeSpace);
        }
    }
}
