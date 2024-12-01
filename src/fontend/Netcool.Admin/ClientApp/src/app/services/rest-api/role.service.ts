import { CrudRestServiceBase } from "./crud-rest.service";
import { Permission, Role, } from "@models";
import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable({
  providedIn: "root"
})
export class RoleService extends CrudRestServiceBase<Role> {
  constructor(protected http: HttpClient) {
    super("api/roles", http);
  }

  getPermissions(id: number): Observable<Permission[]> {
    return this.http.get<Permission[]>(`api/roles/${id}/permissions`);
  }

  setPermissions(id: number, permissionIds: number[]): Observable<any> {
    return this.http.post(`api/roles/${id}/permissions`, permissionIds);
  }
}
