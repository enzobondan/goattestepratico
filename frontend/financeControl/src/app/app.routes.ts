import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { authGuard } from './core/guard/auth.guard';
import { HomeComponent } from './pages/home/home.component';
import { PlansComponent } from './pages/plans/plans.component';
import { TenantComponent } from './pages/tenant/tenant.component';
import { ManagementComponent } from './pages/management/management.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'planos', component: PlansComponent },
  { path: 'login', component: LoginComponent },
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [authGuard],
  },
  {
    path: 'tenant/:id',
    component: TenantComponent,
    canActivate: [authGuard],
  },
  { path: 'tenant/:id/management', component: ManagementComponent, canActivate: [authGuard] },
];
