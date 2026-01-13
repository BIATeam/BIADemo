/* eslint-disable @typescript-eslint/naming-convention */
export enum PropType {
  Date = 'Date',
  DateTime = 'DateTime',
  Time = 'Time', // For dateTime field if you just manipulate Time
  TimeOnly = 'TimeOnly',
  TimeSecOnly = 'TimeSecOnly',
  Number = 'Number',
  Boolean = 'Boolean',
  String = 'String',
  OneToMany = 'OneToMany',
  ManyToMany = 'ManyToMany',
}
