export class Permissions {
  private constructor() {
  }

  private static _instance: Permissions;

  public static get Instance() {
    return this._instance || (this._instance = new this());
  }

  userCreate = "user.create";
  userUpdate = "user.update";
  userDelete = "user.delete";
  userSetRoles = "user.set-roles";

  roleCreate = "role.create";
  roleUpdate = "role.update";
  roleDelete = "role.delete";
  roleSetPermissions = "role.set-permissions";

  configCreate = "config.create";
  configUpdate = "config.update";
  configDelete = "config.delete";

  organizationCreate = "organization.create";
  organizationUpdate = "organization.update";
  organizationDelete = "organization.delete";

  announcementCreate = "announcement.create";
  announcementUpdate = "announcement.update";
  announcementDelete = "announcement.delete";
  announcementPublish = "announcement.publish";

}

