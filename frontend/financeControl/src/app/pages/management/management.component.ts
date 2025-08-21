import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiService } from '../../core/services/api.service';
import { VendorService } from '../../core/services/vendor.service';
import { CostCenterService } from '../../core/services/cost-center.service';
import { ExpenseCategoryService } from '../../core/services/expense-category.service';
import { Vendor, CostCenter, ExpenseCategory } from '../../core/interfaces';

@Component({
  selector: 'app-management',
  templateUrl: './management.component.html',
  styleUrls: ['./management.component.css'],
  imports: [CommonModule, FormsModule],
  standalone: true,
})
export class ManagementComponent implements OnInit {
  tenantId: string = '';
  activeTab: 'vendors' | 'costCenters' | 'categories' = 'vendors';

  vendors: Vendor[] = [];
  costCenters: CostCenter[] = [];
  expenseCategories: ExpenseCategory[] = [];

  showCreateModal = false;
  currentEntity: any = {};
  entityType: 'vendor' | 'costCenter' | 'category' = 'vendor';
  isEditMode: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private apiService: ApiService,
    private vendorService: VendorService,
    private costCenterService: CostCenterService,
    private expenseCategoryService: ExpenseCategoryService
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      this.tenantId = params.get('id') || '';
      if (this.tenantId) {
        this.loadData();
      }
    });
  }

  loadData(): void {
    this.loadVendors();
    this.loadCostCenters();
    this.loadExpenseCategories();
  }

  loadVendors(): void {
    this.vendorService.getVendorsByTenant(this.tenantId).subscribe({
      next: (vendors) => (this.vendors = vendors),
      error: (error) => {
        console.error('Erro ao carregar fornecedores:', error);
        this.showError('Erro ao carregar fornecedores', error);
      },
    });
  }

  loadCostCenters(): void {
    this.costCenterService.getCostCentersByTenant(this.tenantId).subscribe({
      next: (costCenters) => (this.costCenters = costCenters),
      error: (error) => {
        console.error('Erro ao carregar centros de custo:', error);
        this.showError('Erro ao carregar centros de custo', error);
      },
    });
  }

  loadExpenseCategories(): void {
    this.expenseCategoryService.getExpenseCategoriesByTenant(this.tenantId).subscribe({
      next: (categories) => (this.expenseCategories = categories),
      error: (error) => {
        console.error('Erro ao carregar categorias:', error);
        this.showError('Erro ao carregar categorias', error);
      },
    });
  }

  returnBtn() {
    this.router.navigate(['/dashboard']);
  }

  openCreateModal(type: 'vendor' | 'costCenter' | 'category'): void {
    this.entityType = type;
    this.currentEntity = { tenantId: this.tenantId };
    this.isEditMode = false;
    this.showCreateModal = true;
  }

  openEditModal(type: 'vendor' | 'costCenter' | 'category', entity: any): void {
    this.entityType = type;
    this.currentEntity = { ...entity, tenantId: this.tenantId };
    this.isEditMode = true;
    this.showCreateModal = true;
  }

  closeCreateModal(): void {
    this.showCreateModal = false;
    this.currentEntity = {};
    this.isEditMode = false;
  }

  saveEntity(): void {
    if (this.isEditMode) {
      this.updateEntity();
    } else {
      this.createEntity();
    }
  }

  createEntity(): void {
    const entityToSend = {
      ...this.currentEntity,
      tenantId: this.tenantId,
    };

    switch (this.entityType) {
      case 'vendor':
        const vendorData = {
          tenantId: this.tenantId,
          nome: this.currentEntity.nome,
          cnpjCpf: this.currentEntity.cnpjCpf,
          email: this.currentEntity.email || null,
          telefone: this.currentEntity.telefone || null,
          banco: this.currentEntity.banco || null,
          agencia: this.currentEntity.agencia || null,
          contaCorrente: this.currentEntity.contaCorrente || null,
          pix: this.currentEntity.pix || null,
        };

        this.vendorService.createVendor(vendorData).subscribe({
          next: () => {
            this.closeCreateModal();
            this.loadVendors();
          },
          error: (error) => {
            console.error('Erro ao criar fornecedor:', error);
            this.showError('Erro ao criar fornecedor', error);
          },
        });
        break;

      case 'costCenter':
        const costCenterData = {
          tenantId: this.tenantId,
          codigo: entityToSend.codigo,
          nome: entityToSend.nome,
          descricao: entityToSend.descricao || null,
        };

        this.costCenterService.createCostCenter(costCenterData).subscribe({
          next: () => {
            this.closeCreateModal();
            this.loadCostCenters();
          },
          error: (error) => {
            console.error('Erro ao criar centro de custo:', error);
            this.showError('Erro ao criar centro de custo', error);
          },
        });
        break;

      case 'category':
        const categoryData = {
          tenantId: this.tenantId,
          nome: entityToSend.nome,
          descricao: entityToSend.descricao || null,
          codigoContaContabil: entityToSend.codigoContaContabil || null,
        };

        this.expenseCategoryService.createExpenseCategory(categoryData).subscribe({
          next: () => {
            this.closeCreateModal();
            this.loadExpenseCategories();
          },
          error: (error) => {
            console.error('Erro ao criar categoria:', error);
            this.showError('Erro ao criar categoria', error);
          },
        });
        break;
    }
  }

  updateEntity(): void {
    const entityToSend = {
      ...this.currentEntity,
      tenantId: this.tenantId,
    };

    switch (this.entityType) {
      case 'vendor':
        this.vendorService.updateVendor(this.currentEntity.id, entityToSend).subscribe({
          next: () => {
            this.closeCreateModal();
            this.loadVendors();
          },
          error: (error) => {
            console.error('Erro ao atualizar fornecedor:', error);
            this.showError('Erro ao atualizar fornecedor', error);
          },
        });
        break;

      case 'costCenter':
        this.costCenterService.updateCostCenter(this.currentEntity.id, entityToSend).subscribe({
          next: () => {
            this.closeCreateModal();
            this.loadCostCenters();
          },
          error: (error) => {
            console.error('Erro ao atualizar centro de custo:', error);
            this.showError('Erro ao atualizar centro de custo', error);
          },
        });
        break;

      case 'category':
        this.expenseCategoryService
          .updateExpenseCategory(this.currentEntity.id, entityToSend)
          .subscribe({
            next: () => {
              this.closeCreateModal();
              this.loadExpenseCategories();
            },
            error: (error) => {
              console.error('Erro ao atualizar categoria:', error);
              this.showError('Erro ao atualizar categoria', error);
            },
          });
        break;
    }
  }

  deleteEntity(type: 'vendor' | 'costCenter' | 'category', id: string): void {
    if (confirm('Tem certeza que deseja excluir?')) {
      switch (type) {
        case 'vendor':
          this.vendorService.deleteVendor(id).subscribe({
            next: () => this.loadVendors(),
            error: (error) => {
              console.error('Erro ao excluir fornecedor:', error);
              this.showError('Erro ao excluir fornecedor', error);
            },
          });
          break;
        case 'costCenter':
          this.costCenterService.deleteCostCenter(id).subscribe({
            next: () => this.loadCostCenters(),
            error: (error) => {
              console.error('Erro ao excluir centro de custo:', error);
              this.showError('Erro ao excluir centro de custo', error);
            },
          });
          break;
        case 'category':
          this.expenseCategoryService.deleteExpenseCategory(id).subscribe({
            next: () => this.loadExpenseCategories(),
            error: (error) => {
              console.error('Erro ao excluir categoria:', error);
              this.showError('Erro ao excluir categoria', error);
            },
          });
          break;
      }
    }
  }

  private showError(message: string, error: any): void {
    let errorMessage = message;
    if (error.status === 400) {
      errorMessage += ': ' + (error.error?.message || 'Dados inválidos');
    } else if (error.status === 401) {
      errorMessage += ': Sessão expirada. Faça login novamente.';
      this.router.navigate(['/login']);
    } else if (error.status === 403) {
      errorMessage += ': Acesso não autorizado.';
    } else if (error.status === 404) {
      errorMessage += ': Recurso não encontrado.';
    } else {
      errorMessage += ': ' + (error.error?.message || error.message);
    }
    alert(errorMessage);
  }
}
