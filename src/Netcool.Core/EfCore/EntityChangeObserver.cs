using System;
using System.Threading;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Netcool.Core.EfCore
{
    public class EntityChangeEvent
    {
        public object Entity { get; set; }

        public EntityChangeType ChangeType { get; set; }

        public EntityChangeEvent(object entity, EntityChangeType changeType)
        {
            Entity = entity;
            ChangeType = changeType;
        }
    }

    public enum EntityChangeType
    {
        Created,
        Updated,
        Deleted
    }

    public class EntityChangeObserver
    {
        public event EventHandler<EntityChangeEvent> Changed;

        public void OnChanged(EntityChangeEvent e)
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
