export class PagedResult<T> {
  total: number;
  items: T[]
}

export class NameValue<T> {
  name: string;
  value: T;
}

export class Organization {
  id: number;
  name: string;
  description: string;
  parentId: number;
  path: string;
  depth: number;
  parent: Organization;
}

export class Role {
  id: number;
  name: string;
  notes: string;
}

export class User {
  id: string;
  name: string;
  displayName: string;
  gender: number = 1;
  email: string;
  phone: string;
  isActive: boolean = true;
  genderDescription: string;

  roles: Role[]

  [key: string]: any
}

export interface LoginResult {
  user: User;
  accessToken: string;
  expiryAt: Date;
  permissionCodes: string[]
}

export class AppConfig {
  id: number;
  name: string;
  value: string;
  description: string;
  type: number;
}

export class Menu {
  id: number;
  parentId: number;
  name: string;
  displayName: string;
  type: number;
  route: string;
  icon: string;
  level: number;
  order: number;
  path: string;
  notes: string;
  permissions: Permission[]

  [key: string]: any;
}

export class Permission {
  id: number;
  menuId: number;
  name: number;
  code: number;
  notes: string;
  type: number;

  [key: string]: any;
}

export enum PermissionType {
  Menu,
  Function
}

export class UserAnnouncement {
  announcement: Announcement;
  userId: number;
  isRead: boolean;
  readTime: Date;
}

export class UserAnnouncementReadInput {
  userId: number;
  announcementIds: number[];
}

export class Announcement {
  id: number;
  title: string;
  body: string;
  status: number;
  notifyTargetType: number;
  statusDescription: string;
  notifyTargetTypeDescription: string;
  updateTime: Date;
}
