using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Backend.Common.Helpers;
using Ecommerce.Backend.Common.Models;
using Ecommerce.Backend.Entities;
using Ecommerce.Backend.Services.Abstractions;
using MongoDB.Bson;
using Moq;
using Xunit;

namespace Ecommerce.Backend.Services.Tests
{
  public class BaseServiceTest
  {
    private const string _entityId = "5eb3f92920d7467aed27c7c2";
    private List<BaseEntityWithStatus> _entities = new List<BaseEntityWithStatus>();
    private readonly Mock<IBaseService<BaseEntityWithStatus>> _mockBaseService;

    public BaseServiceTest()
    {
      _mockBaseService = new Mock<IBaseService<BaseEntityWithStatus>>(MockBehavior.Strict);
      _entities = new List<BaseEntityWithStatus>();
      _generateBaseEntities();
    }

    private void _generateBaseEntities()
    {
      for (var i = 0; i < 100; i++)
      {
        var entity = new BaseEntityWithStatus
        {
          ID = ObjectId.GenerateNewId().ToString()
        };
        _entities.Add(entity);
      }
      _entities.Add(new BaseEntityWithStatus { ID = _entityId });
    }

    [Theory]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(20)]
    [InlineData(5, 10)]
    [InlineData(10, 8)]
    [InlineData(10, 11)]
    [InlineData(20, 4)]
    public async Task Get_Paginated_Entity_List_With_PageSize_Test(int pageSize, int page = 1)
    {
      // Arrange
      var query = new PagedQuery { PageSize = pageSize, Page = page };
      _mockBaseService.Setup(s => s.GetPaginatedList(It.IsAny<PagedQuery>())).ReturnsAsync(() =>
      {
        var entities = _entities.Skip(query.PageSize * (query.Page - 1)).Take(query.PageSize).ToList();
        return entities.ToPagedList(_entities.Count);
      });

      // Act
      var pagedList = await _mockBaseService.Object.GetPaginatedList(query);

      // Assert
      Assert.NotNull(pagedList);
      Assert.NotNull(pagedList.Items);
      Assert.True(pagedList.Items.IsNotEmpty());
      Assert.True(pagedList.Items.ToList().Count <= query.PageSize);
      Assert.True(pagedList.Items.All(item => !item.IsDeleted));
      Assert.Equal(pagedList.Count, _entities.Count);
      _mockBaseService.Verify((s) => s.GetPaginatedList(query), Times.Once);
    }

    [Theory]
    [InlineData(_entityId)]
    public async Task Get_Entity_By_Id(string entityId)
    {
      // Arrange
      _mockBaseService.Setup(s => s.GetById(It.IsAny<string>())).ReturnsAsync(() =>
      {
        var entity = _entities.FirstOrDefault(x => x.ID == entityId);
        return entity;
      });

      // Act
      var entity = await _mockBaseService.Object.GetById(entityId);

      // Assert
      Assert.NotNull(entity);
      Assert.Equal(entity.ID, entityId);
    }

    [Theory]
    [InlineData("d7b3f92920d7467aed27cc09")]
    public async Task Get_Entity_With_Invalid_Id_Returns_Null(string entityId)
    {
      // Arrange
      _mockBaseService.Setup(s => s.GetById(It.IsAny<string>())).ReturnsAsync(() =>
      {
        var entity = _entities.FirstOrDefault(x => x.ID == entityId);
        return entity;
      });

      // Act
      var entity = await _mockBaseService.Object.GetById(entityId);

      // Assert
      Assert.Null(entity);
    }
  }
}