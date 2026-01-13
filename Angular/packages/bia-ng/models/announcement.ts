import { BaseDto } from './dto/base-dto';
import { VersionedDto } from './dto/versioned-dto';
import { OptionDto } from './option-dto';

export interface Announcement extends BaseDto, VersionedDto {
  end: Date;
  rawContent: string;
  start: Date;
  type: OptionDto;
}
