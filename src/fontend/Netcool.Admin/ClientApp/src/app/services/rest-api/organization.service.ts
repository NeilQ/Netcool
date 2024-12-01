import { CrudRestServiceBase } from "./crud-rest.service";
import { Organization } from "@models";
import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { SFSchemaEnum } from "@delon/form";
import { map } from "rxjs/operators";

@Injectable({
  providedIn: "root"
})
export class OrganizationService extends CrudRestServiceBase<Organization> {
  constructor(protected http: HttpClient) {
    super("api/organizations", http);
  }

  getTreeEnumOptions(): Observable<SFSchemaEnum[]> {
    return this.list({sort: "depth,id"})
      .pipe(map(data => {
        let arr: SFSchemaEnum [] = [{title: '无', key: 0, isLeaf: true}];
        let map = new Map<number, SFSchemaEnum>();
        data.forEach((value) => {
          if (value.parentId == null || value.parentId <= 0) {
            let node = {
              title: value.name,
              key: value.id,
              children: [],
              isLeaf: true
            };
            map.set(value.id, node);
            arr.push(node);
          } else {
            let node = {
              title: value.name,
              key: value.id,
              children: [],
              isLeaf: true
            }
            let parentNode = map.get(value.parentId);
            if (parentNode != null) {
              parentNode.isLeaf = false;
              parentNode.children.push(node);
            }
            map.set(value.id, node);
          }
        });
        return arr;
      }));
  }
}
