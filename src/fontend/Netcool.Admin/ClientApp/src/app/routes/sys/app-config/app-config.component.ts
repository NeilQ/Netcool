import { Component } from '@angular/core';
import { AppConfigService, NotificationService } from "@services";
import { AppConfig, CrudTableComponentBase } from "@models";
import { SysAppConfigEditComponent } from "./edit/edit.component";
import { ModalHelper } from "@delon/theme";
import { STColumnTag } from "@delon/abc/st";
import { SFSchema } from "@delon/form";

const TAG: STColumnTag = {
  true: {text: '系统初始化', color: 'green'},
  false: {text: '', color: ''},
};

@Component({
  selector: 'sys-app-config',
  templateUrl: './app-config.component.html',
})
export class SysAppConfigComponent extends CrudTableComponentBase<AppConfig> {

  searchSchema: SFSchema = {
    properties: {
      name: {type: 'string', title: '账号名称', maxLength: 32},
    }
  };

  constructor(protected apiService: AppConfigService,
              protected modal: ModalHelper,
              protected notificationService: NotificationService) {
    super(apiService, modal, notificationService)
    this.editComponent = SysAppConfigEditComponent;
    this.columns = [
      {title: "id", index: "id", type: "checkbox"},
      {title: "名称", index: "name", width: "200px"},
      {title: "值", index: "value", width: "200px"},
      {title: "说明", index: 'description'},
      {title: "是否系统初始化", index: "isInitial", width: "200px", type: "tag", tag: TAG},
      {
        title: "操作", width: "200px",
        buttons: [
          {
            text: "编辑", icon: "edit", type: "modal",
            modal: {component: this.editComponent, params: (record) => record},
            acl: this.permissions.configUpdate,
            click: () => this.onSaveSuccess()
          }
        ]
      }
    ];

  }


}
