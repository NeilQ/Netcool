import { CrudRestServiceBase } from "./crud-rest.service";
import { LoginResult, Role, User } from "@models";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { HttpClient, HttpContext } from "@angular/common/http";
import { ALLOW_ANONYMOUS } from "@delon/auth";

@Injectable({
  providedIn: "root"
})
export class UserService extends CrudRestServiceBase<User> {
  constructor(protected http: HttpClient) {
    super("api/users", http);
  }

  login(body: { name: string, password: string }): Observable<LoginResult> {
    return this.http.post<LoginResult>("api/account/authenticate", body, {
      context: new HttpContext().set(ALLOW_ANONYMOUS, true)
    });
  }

  getRoles(id: number): Observable<Role[]> {
    return this.http.get<Role[]>(`api/users/${id}/roles`)
  }

  setRoles(id: number, roleIds: number[]): Observable<any> {
    return this.http.post(`api/users/${id}/roles`, roleIds || [])
  }

  resetPassword(id: number, newPassword: string, confirmPassword: string): Observable<any> {
    return this.http.post(`api/users/${id}/password/reset`, {new: newPassword, confirm: confirmPassword});
  }
}
