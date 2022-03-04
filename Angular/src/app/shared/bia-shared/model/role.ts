export interface RoleDto {
  id: number;
  code: string;
  label: string;
  isDefault: boolean;
  roleTranslations: RoleTranslationDto;
}

export interface RoleTranslationDto {
  languageId: number;
  label: string;
}
