export interface RoleDto {
  id: number;
  code: string;
  label: string;
  roleTranslations: RoleTranslationDto;
}

export interface RoleTranslationDto {
  languageId: number;
  label: string;
}
