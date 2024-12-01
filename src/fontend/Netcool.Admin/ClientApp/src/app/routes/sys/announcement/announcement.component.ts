import { Component, OnInit } from '@angular/core';
import { SFSchema } from '@delon/form';
import { ModalHelper } from '@delon/theme';
import { Announcement, CrudTableComponentBase } from "@models";
import { AnnouncementService, EnumService, NotificationService } from "@services";
import { SysAnnouncementEditComponent } from "./edit/edit.component";
import { SysAnnouncementViewComponent } from "./view/view.component";

@Component({
  selector: 'sys-announcement',
  templateUrl: './announcement.component.html',
})
export class SysAnnouncementComponent extends CrudTableComponentBase<Announcement> implements OnInit {

  searchSchema: SFSchema = {
    properties: {
      title: {type: 'string', title: '标题', maxLength: 32},
      status: {
        type: 'number', title: '状态',
        ui: {
          widget: 'select',
          allowClear: true
        },
        enum: this.enumService.getEnum('announcementStatus').map(t => ({
          label: t.name,
          value: t.value
        }))
      },
    }
  };

  constructor(protected apiService: AnnouncementService,
              protected modal: ModalHelper,
              protected notificationService: NotificationService,
              private enumService: EnumService) {
    super(apiService, modal, notificationService)
    this.editComponent = SysAnnouncementEditComponent;
    this.columns = [
      {title: "id", index: "id", type: "checkbox"},
      {title: '标题', index: 'title'},
      {title: '状态', index: 'statusDescription', width: '120px'},
      {title: '通知对象', index: 'notifyTargetTypeDescription', width: '120px'},
      {title: '最后更新时间', index: 'updateTime', type: 'date', width: '150px'},
      {
        title: "操作", width: "200px",
        buttons: [
          {
            text: "编辑", icon: "edit", type: "modal",
            modal: {component: this.editComponent, params: (record) => record, modalOptions: {nzKeyboard: false}},
            acl: this.permissions.announcementUpdate,
            iif: record => record.status === 0,
            click: () => this.onSaveSuccess()
          },
          {
            text: "预览", type:"modal",
            modal: {component: SysAnnouncementViewComponent, params: (id) => id, modalOptions: {nzKeyboard: false}},
            click: () => this.onSaveSuccess()
          },
          {
            text: "发布",
            iif: record => record.status === 0,
            acl: this.permissions.announcementPublish,
            pop: {
              title: '确定要发布该公告吗？',
              okType: 'danger'
            }, click: (record) => {
              this.publish(record.id)
            },
          }
        ]
      }
    ];

  }

  publish(id: number) {
    this.apiService.publish(id).subscribe(() => {
      this.notificationService.successMessage("发布成功");
      this.loadLazy();
    })
  }

}
