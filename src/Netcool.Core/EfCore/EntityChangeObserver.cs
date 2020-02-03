using System;
using System.Threading;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Netcool.Core.EfCore
{
    public class EntityChangeEventArgs
    {
        public EntityEntry Entry { get; set; }

        public EntityChangeEventArgs(EntityEntry entry)
        {
            Entry = entry;
        }
    }

    public class EntityChangeObserver
    {
        public event EventHandler<EntityChangeEventArgs> Changed;

        public void OnChanged(EntityChangeEventArgs e)
        {
            ThreadPool.QueueUserWorkItem((_) => Changed?.Invoke(this, e));
        }

        #region singleton

        private static readonly Lazy<EntityChangeObserver> Lazy =
            new Lazy<EntityChangeObserver>(() => new EntityChangeObserver());

        private EntityChangeObserver()
        {
        }

        public static EntityChangeObserver Instance => Lazy.Value;

        #endregion singleton
    }
}