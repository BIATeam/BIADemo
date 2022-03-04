export interface RoleDto {
  id: number;
  code: string;
  display: string;
  isDefault: boolean;
}

export interface RoleTranslationDto {
  languageId: number;
  label: string;
}
