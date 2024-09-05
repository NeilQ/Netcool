import { CrudRestServiceBase } from "./crud-rest.service";
import { Menu } from "@models";
import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";

@Injectable({
  providedIn: "root"
})
export class MenuService extends CrudRestServiceBase<Menu> {
  constructor(protected http: HttpClient) {
    super("api/menus", http);
  }

}
