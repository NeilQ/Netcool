import { Injectable, Inject, Provider, APP_INITIALIZER } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ALAIN_I18N_TOKEN, MenuService, SettingsService, TitleService } from '@delon/theme';
import { DA_SERVICE_TOKEN, ITokenService } from '@delon/auth';
import { ACLService } from '@delon/acl';

import { NzIconService } from 'ng-zorro-antd/icon';
import { ICONS_AUTO } from '../../../style-icons-auto';
import { ICONS } from '../../../style-icons';
import { Observable, zip, catchError, map } from "rxjs";
import { EnumService } from "@services";
import { I18NService } from "@core/i18n/i18n.service";
import { NzSafeAny } from "ng-zorro-antd/core/types";
import { Router } from "@angular/router";

/**
 * Used for application startup
 * Generally used to get the basic data of the application, like: Menu Data, User Data, etc.
 */

export function provideStartup(): Provider[] {
  return [
    StartupService,
    {
      provide: APP_INITIALIZER,
      useFactory: (startupService: StartupService) => () => startupService.load(),
      deps: [StartupService],
      multi: true
    }
  ];
}

@Injectable()
export class StartupService {
  constructor(
    iconSrv: NzIconService,
    private menuService: MenuService,
    private enumService: EnumService,
    private settingService: SettingsService,
    private aclService: ACLService,
    private titleService: TitleService,
    @Inject(DA_SERVICE_TOKEN) private tokenService: ITokenService,
    @Inject(ALAIN_I18N_TOKEN) private i18n: I18NService,
    private router: Router,
    private httpClient: HttpClient,
  ) {
    iconSrv.addIcon(...ICONS_AUTO, ...ICONS);
  }

  load(): Observable<void> {
    // only works with promises
    // https://github.com/angular/angular/issues/15088

    const defaultLang = this.i18n.defaultLang;
    let user = this.settingService.user;
    return zip(this.i18n.loadLangData(defaultLang), this.httpClient.get(`api/users/${user.id}/menus/tree`)).pipe(
      // 接收其他拦截器后产生的异常消息
      catchError(res => {
        console.warn(`StartupService.load: Network request failed`, res);
        setTimeout(() => this.router.navigateByUrl(`/exception/500`));
        return [];
      }),
      map(([langData, menuTree]: [Record<string, string>, NzSafeAny]) => {
        // setting language data
        this.i18n.use(defaultLang, langData);

        // 应用信息：包括站点名、描述、年份
        const app: any = {
          name: `Netcool.Admin`,
          description: `Netcool.Admin front-end.`
        };
        this.settingService.setApp(app);

        // 设置页面标题的后缀
        this.titleService.default = '';
        this.titleService.prefix = app.name;
        //this.settingService.setApp(appData.app);

        this.enumService.loadEnums();

        // 用户信息：包括姓名、头像、邮箱地址

        this.aclService.setFull(false)
        this.aclService.setAbility(user.permissionCodes);
        // 初始化菜单
        let rootMenu = {
          text: '导航',
          group: true,
          children: []
        };
        this.appendChildren(rootMenu, menuTree);
        this.menuService.add([rootMenu]);

      })
    );
  }

  private appendChildren(parent, tree): void {
    if (tree == null || tree.children == null || tree.children.length == 0) {
      return;
    }
    for (let i = 0; i < tree.children.length; i++) {
      let item = tree.children[i];
      let menu = {
        group: true,
        text: item.displayName,
        link: (parent.link || '') + item.route,
        icon: {type: 'icon', value: item.icon},
        children: []
      };
      parent.children.push(menu);
      if (item.children != null && item.children.length > 0) {
        menu.group = false;
        this.appendChildren(menu, item)
      } else {
        menu.group = true;
      }
    }
  }

}
