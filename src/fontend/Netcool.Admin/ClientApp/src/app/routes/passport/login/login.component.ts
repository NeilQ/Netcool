import { SettingsService } from '@delon/theme';
import { ChangeDetectionStrategy, Component, Inject, Optional } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { UntypedFormGroup, UntypedFormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalService } from 'ng-zorro-antd/modal';
import { ITokenService, DA_SERVICE_TOKEN } from '@delon/auth';
import { StartupService } from '@core';
import { UserService } from "@services";
import { HttpClient } from "@angular/common/http";
import { finalize } from "rxjs/operators";
import { ReuseTabService } from "@delon/abc/reuse-tab";

import { NzAlertModule } from 'ng-zorro-antd/alert';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzTabsModule } from 'ng-zorro-antd/tabs';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';

@Component({
  selector: 'passport-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NzCheckboxModule,
    NzTabsModule,
    NzAlertModule,
    NzFormModule,
    NzInputModule,
    NzButtonModule,
    NzToolTipModule,
    NzIconModule
  ]
})
export class UserLoginComponent {

  loading = false;

  constructor(
    fb: UntypedFormBuilder,
    modalSrv: NzModalService,
    private router: Router,
    private settingsService: SettingsService,
    @Optional()
    @Inject(ReuseTabService)
    private reuseTabService: ReuseTabService,
    @Inject(DA_SERVICE_TOKEN) private tokenService: ITokenService,
    private startupSrv: StartupService,
    private userService: UserService,
    public http: HttpClient,
    public msg: NzMessageService,
  ) {
    this.form = fb.group({
      userName: [null, [Validators.required, Validators.minLength(1)]],
      password: [null, Validators.required],
      mobile: [null, [Validators.required, Validators.pattern(/^1\d{10}$/)]],
      captcha: [null, [Validators.required]],
      remember: [true],
    });
    modalSrv.closeAll();
  }

  get userName() {
    return this.form.controls.userName;
  }

  get password() {
    return this.form.controls.password;
  }

  get mobile() {
    return this.form.controls.mobile;
  }

  form: UntypedFormGroup;
  error = '';

  submit() {
    this.error = '';
    this.userName.markAsDirty();
    this.userName.updateValueAndValidity();
    this.password.markAsDirty();
    this.password.updateValueAndValidity();
    if (this.userName.invalid || this.password.invalid) {
      return;
    }

    this.loading = true;
    // 默认配置中对所有HTTP请求都会强制 [校验](https://ng-alain.com/auth/getting-started) 用户 Token
    // 然一般来说登录请求不需要校验，因此可以在请求URL加上：`/login?_allow_anonymous=true` 表示不触发用户 Token 校验
    this.userService.login({name: this.userName.value, password: this.password.value})
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe((res) => {
        // 清空路由复用信息
        this.reuseTabService.clear();
        // 设置用户Token信息
        this.tokenService.set({token: res.accessToken});
        this.settingsService.setUser({
          id: res.user.id,
          name: res.user.displayName || res.user.name,
          avatar: null,
          email: res.user.email,
          permissionCodes: res.permissionCodes
        });

        // 重新获取 StartupService 内容，我们始终认为应用信息一般都会受当前用户授权范围而影响
        this.startupSrv.load().subscribe(() => {
          let url = this.tokenService.referrer!.url || '/';
          if (url.includes('/passport')) {
            url = '/';
          }
          this.router.navigateByUrl(url);
        });
      });
  }
}
