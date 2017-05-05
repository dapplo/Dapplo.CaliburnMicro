using System.Collections.Generic;
using Dapplo.CaliburnMicro.Menu;
using Xunit;

namespace Dapplo.Caliburn.Tests
{
    public class BasicTests
    {
        [Fact]
        public void Test_Tree()
        {
            var item1 = new MenuItem
            {
                Id = "A_1",
            };
            var item11 = new MenuItem
            {
                Id = "A_1_1",
                ParentId = "A_1",
            };
            var item12 = new MenuItem
            {
                Id = "A_1_2",
                ParentId = "A_1",
            };
            var item0 = new MenuItem
            {
                Id = "A_0",
            };
            var items = new List<IMenuItem> {
                item11, item1, item12, item0
            };

            bool wasA0AlreadySeen = false;

            var tree = items.CreateTree();
            foreach (var treeNode in tree)
            {
                if (treeNode.Id == "A_1")
                {
                    Assert.True(treeNode.ChildNodes.Count == 2);
                }
                if (treeNode.Id == "A_1")
                {
                    Assert.True(wasA0AlreadySeen);
                }
                if (treeNode.Id == "A_0")
                {
                    wasA0AlreadySeen = true;
                }
            }
        }
    }
}
