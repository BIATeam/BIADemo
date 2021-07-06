import { Site } from 'src/app/domains/site/model/site';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';

export interface Plane {
  id: number;
  msn: string;
  isActive: boolean;
  firstFlightDate: Date;
  firstFlightTime: Date;
  lastFlightDate: Date;
  capacity: number;
  site: Site[];
  connectingAirports: OptionDto[];
  planeType: OptionDto | null;
}

