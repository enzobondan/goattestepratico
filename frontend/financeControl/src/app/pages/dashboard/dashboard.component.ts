import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../core/services/auth.service';
import { ApiService } from '../../core/services/api.service';
import { User, Tenant } from '../../core/interfaces';
import { TenantControlComponent } from '../../components/tenant-control/tenant-control.component';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
  imports: [CommonModule, RouterModule, FormsModule, TenantControlComponent],
  standalone: true,
})
export class DashboardComponent implements OnInit {
  currentUser: User | null = null;
  tenants: Tenant[] = [];
  isLoading = true;
  errorMessage = '';
  showTenantModal = false;
  isEditMode = false;
  selectedTenant: Tenant = {
    accountId: '',
    id: '',
    razaoSocial: '',
    cnpj: '',
    nomeFantasia: '',
    limiteAprovacaoAutomatica: 0,
    ativo: true,
  };

  constructor(
    private authService: AuthService,
    private apiService: ApiService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadDashboardData();
  }

  logout() {
    this.authService.logout();
  }

  loadDashboardData(): void {
    this.isLoading = true;

    this.authService.getCurrentUser().subscribe({
      next: (user) => {
        this.currentUser = user;
        this.loadTenants();
      },
      error: (error) => {
        console.error('Erro ao carregar usuário:', error);
        this.isLoading = false;
        this.errorMessage = 'Erro ao carregar dados do usuário';
      },
    });
  }

  loadTenants(): void {
    this.apiService.get<Tenant[]>('Tenants').subscribe({
      next: (tenants) => {
        this.tenants = tenants;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Erro ao carregar tenants:', error);
        this.isLoading = false;
        this.errorMessage = 'Erro ao carregar tenants';
      },
    });
  }

  selectTenant(tenant: Tenant): void {
    this.router.navigate(['/tenant', tenant.id]);
  }

  editTenant(tenant: Tenant): void {
    this.selectedTenant = { ...tenant };
    this.isEditMode = true;
    this.showTenantModal = true;
  }

  createTenant(): void {
    this.selectedTenant = {
      accountId: '',
      id: '',
      razaoSocial: '',
      cnpj: '',
      nomeFantasia: '',
      limiteAprovacaoAutomatica: 0,
      ativo: true,
    };
    this.isEditMode = false;
    this.showTenantModal = true;
  }

  saveTenant(tenant: Tenant): void {
    if (this.isEditMode) {
      this.updateTenant(tenant);
    } else {
      this.createNewTenant(tenant);
    }
  }

  createNewTenant(tenant: Tenant): void {
    const tenantData = {
      razaoSocial: tenant.razaoSocial,
      cnpj: tenant.cnpj.replace(/\D/g, ''),
      limiteAprovacaoAutomatica: tenant.limiteAprovacaoAutomatica,
    };
    console.log(tenantData);

    this.apiService.post<Tenant>('Tenants', tenantData).subscribe({
      next: (newTenant) => {
        this.tenants.push(newTenant);
        this.showTenantModal = false;
        this.errorMessage = '';
      },
      error: (error) => {
        console.error('Erro ao criar tenant:', error);
        this.errorMessage = 'Erro ao criar tenant';
      },
    });
  }

  updateTenant(tenant: Tenant): void {
    const tenantData = {
      id: tenant.id,
      razaoSocial: tenant.razaoSocial,
      cnpj: tenant.cnpj.replace(/\D/g, ''),
      limiteAprovacaoAutomatica: tenant.limiteAprovacaoAutomatica,
      ativo: tenant.ativo,
    };

    console.log('Atualizando tenant:', tenantData);

    this.apiService.put<Tenant>(`Tenants/${tenant.id}`, tenantData).subscribe({
      next: (updatedTenant) => {
        const index = this.tenants.findIndex((t) => t.id === updatedTenant.id);
        if (index !== -1) {
          this.tenants[index] = updatedTenant;
        }
        this.showTenantModal = false;
        this.errorMessage = '';
      },
      error: (error) => {
        console.error('Erro ao atualizar tenant:', error);
        this.errorMessage = 'Erro ao atualizar tenant';
      },
    });
  }

  deleteTenant(tenantId: string): void {
    this.apiService.delete(`Tenants/${tenantId}`).subscribe({
      next: () => {
        this.tenants = this.tenants.filter((t) => t.id !== tenantId);
        this.showTenantModal = false;
        this.errorMessage = '';
      },
      error: (error) => {
        console.error('Erro ao excluir tenant:', error);
        this.errorMessage = 'Erro ao excluir tenant';
      },
    });
  }

  closeModal(): void {
    this.showTenantModal = false;
  }

  isAdmin(): boolean {
    return this.currentUser?.accountRole !== null && this.currentUser?.accountRole !== undefined;
  }
}
