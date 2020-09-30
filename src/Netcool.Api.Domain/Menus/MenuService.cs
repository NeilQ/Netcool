using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Netcool.Core.Entities;
using Netcool.Core.Repositories;
using Netcool.Core.Services;
using Netcool.Core.Services.Dto;

namespace Netcool.Api.Domain.Menus
{
    public class MenuService : CrudService<Menu, MenuDto, int, PageRequest>, IMenuService
    {
        public MenuService(IRepository<Menu, int> repository, IServiceAggregator serviceAggregator) : base(repository,
            serviceAggregator)
        {
        }

        protected override IQueryable<Menu> CreateFilteredQuery(PageRequest input)
        {
            return base.CreateFilteredQuery(input).Include(t => t.Permissions);
        }

        public override void BeforeUpdate(MenuDto input, Menu originEntity)
        {
            if (input.ParentId == originEntity.ParentId) return;
            if (input.Level <= 1)
            {
                input.Path = $"/{input.Id}";
            }
            else
            {
                var parent = GetEntityById(input.ParentId);
                if (parent == null) throw new EntityNotFoundException("父节点id不存在");
                input.Path = $"{parent.Path}/{input.Id}";
            }
        }

        public MenuTreeNode GetMenuTree()
        {
            var menus = Repository.GetAll().OrderBy(t => t.Level).AsNoTracking().ToList();
            if (menus.Count == 0) return null;
            var rootNode = new MenuTreeNode();
            var dict = new Dictionary<int, MenuTreeNode>
            {
                {0, rootNode}
            };
            foreach (var menu in menus)
            {
                if (dict.ContainsKey(menu.Id)) continue; // ignore duplicate menu node
                var treeNode = MapToEntityDto<Menu, MenuTreeNode>(menu);
                dict.Add(menu.Id, treeNode);

                if (menu.ParentId > 0 && menu.Level > 1)
                {
                    if (!dict.TryGetValue(menu.ParentId, out var parentNode))
                        continue; // ignore menu node with invalid parentId

                    if (parentNode.Children == null) parentNode.Children = new List<MenuTreeNode>();
                    parentNode.Children.Add(treeNode);
                }
                else
                {
                    if (rootNode.Children == null) rootNode.Children = new List<MenuTreeNode>();
                    rootNode.Children.Add(treeNode);
                }
            }

            return rootNode;
        }
    }
}
