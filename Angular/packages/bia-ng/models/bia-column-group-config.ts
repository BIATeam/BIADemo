export interface BiaColumnGroupCell {
  header: string;
  fieldKeys: string[];
}

export interface BiaColumnGroupConfig {
  rows: BiaColumnGroupCell[][];
}
