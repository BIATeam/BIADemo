import { RoleMode, TeamTypeId } from 'src/app/shared/constants';

export const allEnvironments = {
    appTitle: 'BIADemo',
    companyName: 'TheBIADevCompany',
    enableNotifications: true,
    urlAuth: '/api/Auth',
    urlLog: '/api/logs',
    urlEnv: '/api/Environment',
    urlAppIcon: 'assets/bia/AppIcon.svg',
    version: '0.0.0',
    teams: [
        {teamTypeId: TeamTypeId.Site, roleMode: RoleMode.SingleRole, inHeader: true},
        // Begin BIADemo
        {teamTypeId: TeamTypeId.AircraftMaintenanceCompany, roleMode: RoleMode.MultiRoles, inHeader: true},
        // End BIADemo
    ],
};