import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../core/services/api.service';
import { AuthService } from '../../core/services/auth.service';
import {
  FinancialObligation,
  Vendor,
  CostCenter,
  ExpenseCategory,
} from '../../core/interfaces/index';

@Component({
  selector: 'app-tenant',
  templateUrl: './tenant.component.html',
  styleUrls: ['./tenant.component.css'],
  imports: [CommonModule, FormsModule],
  standalone: true,
})
export class TenantComponent implements OnInit {
  tenantId: string = '';
  obligations: FinancialObligation[] = [];
  filteredObligations: FinancialObligation[] = [];
  vendors: Vendor[] = [];
  costCenters: CostCenter[] = [];
  expenseCategories: ExpenseCategory[] = [];
  isLoading = true;
  errorMessage = '';
  showCreateModal = false;
  showXmlUpload = false;
  xmlFile: File | null = null;
  isEditMode = false;
  editingObligationId: string | null = null;

  // Filtros
  filters = {
    status: '',
    vendorId: '',
    costCenterId: '',
    expenseCategoryId: '',
    search: '',
  };

  // Opções para os selects
  statusOptions = [
    { value: '', label: 'Todos os status' },
    { value: 'pendente', label: 'Pendente' },
    { value: 'em_aprovacao', label: 'Em Aprovação' },
    { value: 'aprovado', label: 'Aprovado' },
    { value: 'aguardando_pagamento', label: 'Aguardando Pagamento' },
    { value: 'pago', label: 'Pago' },
    { value: 'cancelado', label: 'Cancelado' },
    { value: 'vencido', label: 'Vencido' },
  ];

  formaPagamentoOptions = [
    { value: '', label: 'Selecione a forma de pagamento' },
    { value: 'dinheiro', label: 'Dinheiro' },
    { value: 'boleto', label: 'Boleto' },
    { value: 'pix', label: 'PIX' },
    { value: 'transferencia', label: 'Transferência Bancária' },
    { value: 'cartao_credito', label: 'Cartão de Crédito' },
    { value: 'cartao_debito', label: 'Cartão de Débito' },
    { value: 'cheque', label: 'Cheque' },
    { value: 'outro', label: 'Outro' },
  ];

  newObligation: any = {
    vendorId: '',
    descricao: '',
    valorNominal: 0,
    dataEmissao: new Date().toISOString().split('T')[0],
    dataVencimento: '',
    numeroNotaFiscal: '',
    serieNotaFiscal: '',
    chaveAcessoNFe: '',
    costCenterId: null,
    expenseCategoryId: null,
    valorICMS: 0,
    valorIPI: 0,
    valorPIS: 0,
    valorCOFINS: 0,
    valorTotalImpostos: 0,
    status: 'pendente',
    formaPagamento: '',
    dataPagamento: '',
    protocoloPagamento: '',
  };

  currencyInputValue: string = '';
  expandedId: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private apiService: ApiService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      this.tenantId = params.get('id') || '';
      if (this.tenantId) {
        this.loadTenantData();
      }
    });
  }

  loadTenantData(): void {
    this.isLoading = true;

    this.apiService
      .get<FinancialObligation[]>(`FinancialObligations/tenant/${this.tenantId}`)
      .subscribe({
        next: (obligations) => {
          this.obligations = obligations;
          this.applyFilters();
          this.loadSupportingData();
        },
        error: (error) => {
          console.error('Erro ao carregar obrigações:', error);
          this.isLoading = false;
          this.errorMessage = 'Erro ao carregar contas a pagar';
        },
      });
  }

  applyFilters(): void {
    this.filteredObligations = this.obligations.filter((obligation) => {
      const matchesStatus = !this.filters.status || obligation.status === this.filters.status;
      const matchesVendor = !this.filters.vendorId || obligation.vendorId === this.filters.vendorId;
      const matchesCostCenter =
        !this.filters.costCenterId || obligation.costCenterId === this.filters.costCenterId;
      const matchesCategory =
        !this.filters.expenseCategoryId ||
        obligation.expenseCategoryId === this.filters.expenseCategoryId;
      const matchesSearch =
        !this.filters.search ||
        obligation.descricao.toLowerCase().includes(this.filters.search.toLowerCase()) ||
        obligation.vendorNome.toLowerCase().includes(this.filters.search.toLowerCase()) ||
        obligation.numeroNotaFiscal?.toLowerCase().includes(this.filters.search.toLowerCase());

      return (
        matchesStatus && matchesVendor && matchesCostCenter && matchesCategory && matchesSearch
      );
    });
  }

  onFilterChange(): void {
    this.applyFilters();
  }

  clearFilters(): void {
    this.filters = {
      status: '',
      vendorId: '',
      costCenterId: '',
      expenseCategoryId: '',
      search: '',
    };
    this.applyFilters();
  }

  toggleDetails(id: string) {
    this.expandedId = this.expandedId === id ? null : id;
  }

  returnBtn() {
    this.router.navigate(['/dashboard']);
  }

  tenantManagement() {
    this.router.navigate(['/tenant', this.tenantId, 'management']);
  }

  loadSupportingData(): void {
    this.apiService.get<Vendor[]>(`Vendors/tenant/${this.tenantId}`).subscribe({
      next: (vendors) => {
        this.vendors = vendors;
      },
      error: (error) => {
        console.error('Erro ao carregar fornecedores:', error);
      },
    });

    this.apiService.get<CostCenter[]>(`CostCenters/tenant/${this.tenantId}`).subscribe({
      next: (costCenters) => {
        this.costCenters = costCenters;
      },
      error: (error) => {
        console.error('Erro ao carregar centros de custo:', error);
      },
    });

    this.apiService.get<ExpenseCategory[]>(`ExpenseCategories/tenant/${this.tenantId}`).subscribe({
      next: (expenseCategories) => {
        this.expenseCategories = expenseCategories;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Erro ao carregar categorias:', error);
        this.isLoading = false;
      },
    });
  }

  openCreateModal(obligation?: FinancialObligation): void {
    this.isEditMode = !!obligation;
    this.editingObligationId = obligation?.id || null;

    if (obligation) {
      this.newObligation = {
        vendorId: obligation.vendorId,
        descricao: obligation.descricao,
        valorNominal: obligation.valorNominal,
        dataEmissao: new Date(obligation.dataEmissao).toISOString().split('T')[0],
        dataVencimento: new Date(obligation.dataVencimento).toISOString().split('T')[0],
        numeroNotaFiscal: obligation.numeroNotaFiscal || '',
        serieNotaFiscal: obligation.serieNotaFiscal || '',
        chaveAcessoNFe: obligation.chaveAcessoNFe || '',
        costCenterId: obligation.costCenterId || null,
        expenseCategoryId: obligation.expenseCategoryId || null,
        valorICMS: obligation.valorICMS || 0,
        valorIPI: obligation.valorIPI || 0,
        valorPIS: obligation.valorPIS || 0,
        valorCOFINS: obligation.valorCOFINS || 0,
        valorTotalImpostos: obligation.valorTotalImpostos || 0,
        status: obligation.status,
        formaPagamento: obligation.formaPagamento || '',
        dataPagamento: obligation.dataPagamento
          ? new Date(obligation.dataPagamento).toISOString().split('T')[0]
          : '',
        protocoloPagamento: obligation.protocoloPagamento || '',
      };
      this.currencyInputValue = this.formatCurrency(obligation.valorNominal);
    } else {
      this.resetNewObligation();
    }

    this.showCreateModal = true;
  }

  closeCreateModal(): void {
    this.showCreateModal = false;
    this.showXmlUpload = false;
    this.isEditMode = false;
    this.editingObligationId = null;
    this.resetNewObligation();
  }

  resetNewObligation(): void {
    this.newObligation = {
      vendorId: '',
      descricao: '',
      valorNominal: 0,
      dataEmissao: new Date().toISOString().split('T')[0],
      dataVencimento: '',
      numeroNotaFiscal: '',
      serieNotaFiscal: '',
      chaveAcessoNFe: '',
      costCenterId: null,
      expenseCategoryId: null,
      valorICMS: 0,
      valorIPI: 0,
      valorPIS: 0,
      valorCOFINS: 0,
      valorTotalImpostos: 0,
      status: 'pendente',
      formaPagamento: '',
      dataPagamento: '',
      protocoloPagamento: '',
    };
    this.currencyInputValue = '';
  }

  approveObligation(obligationId: string): void {
    if (confirm('Tem certeza que deseja aprovar esta conta?')) {
      this.apiService.post(`FinancialObligations/${obligationId}/approve`, {}).subscribe({
        next: () => {
          this.loadTenantData();
          this.errorMessage = '';
        },
        error: (error) => {
          console.error('Erro ao aprovar obrigação:', error);
          this.errorMessage = 'Erro ao aprovar conta';
        },
      });
    }
  }

  toggleXmlUpload(): void {
    this.showXmlUpload = !this.showXmlUpload;
  }

  onXmlFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file && file.type === 'text/xml') {
      this.xmlFile = file;
      this.parseXmlFile(file);
    } else {
      this.errorMessage = 'Por favor, selecione um arquivo XML válido';
    }
  }

  parseXmlFile(file: File): void {
    const reader = new FileReader();
    reader.onload = (e) => {
      try {
        const xmlText = e.target?.result as string;
        const parser = new DOMParser();
        const xmlDoc = parser.parseFromString(xmlText, 'text/xml');

        this.extractDataFromXml(xmlDoc);
      } catch (error) {
        console.error('Erro ao parsear XML:', error);
        this.errorMessage = 'Erro ao ler o arquivo XML';
      }
    };
    reader.readAsText(file);
  }

  extractDataFromXml(xmlDoc: Document): void {
    try {
      const ns = 'http://www.portalfiscal.inf.br/nfe';

      const ide = xmlDoc.getElementsByTagNameNS(ns, 'ide')[0];
      const emit = xmlDoc.getElementsByTagNameNS(ns, 'emit')[0];
      const total = xmlDoc.getElementsByTagNameNS(ns, 'total')[0];
      const cobr = xmlDoc.getElementsByTagNameNS(ns, 'cobr')[0];
      const pag = xmlDoc.getElementsByTagNameNS(ns, 'pag')[0];

      const vendorCnpj = this.getTagValueNS(emit, ns, 'CNPJ');
      const vendorNome = this.getTagValueNS(emit, ns, 'xNome');

      const nNF = this.getTagValueNS(ide, ns, 'nNF');
      const serie = this.getTagValueNS(ide, ns, 'serie');
      const chaveNFe =
        xmlDoc.getElementsByTagNameNS(ns, 'infNFe')[0]?.getAttribute('Id')?.replace('NFe', '') ||
        '';
      const dhEmi = this.getTagValueNS(ide, ns, 'dhEmi');
      const dataEmissao = dhEmi ? new Date(dhEmi).toISOString().split('T')[0] : '';

      const ICMSTot = total.getElementsByTagNameNS(ns, 'ICMSTot')[0];
      const valorTotal = parseFloat(this.getTagValueNS(ICMSTot, ns, 'vNF') || '0');
      const valorICMS = parseFloat(this.getTagValueNS(ICMSTot, ns, 'vICMS') || '0');
      const valorIPI = parseFloat(this.getTagValueNS(ICMSTot, ns, 'vIPI') || '0');
      const valorPIS = parseFloat(this.getTagValueNS(ICMSTot, ns, 'vPIS') || '0');
      const valorCOFINS = parseFloat(this.getTagValueNS(ICMSTot, ns, 'vCOFINS') || '0');

      let dataVencimento = '';
      const dup = cobr?.getElementsByTagNameNS(ns, 'dup')[0];
      if (dup) {
        const dVenc = this.getTagValueNS(dup, ns, 'dVenc');
        dataVencimento = dVenc ? new Date(dVenc).toISOString().split('T')[0] : '';
      }

      let formaPagamento = '';
      const detPag = pag?.getElementsByTagNameNS(ns, 'detPag')[0];
      if (detPag) {
        const tPag = this.getTagValueNS(detPag, ns, 'tPag');
        const formasPagamentoMap: { [key: string]: string } = {
          '01': 'dinheiro',
          '02': 'cheque',
          '03': 'cartao_credito',
          '04': 'cartao_debito',
          '05': 'transferencia',
          '15': 'boleto',
          '16': 'pix',
        };
        formaPagamento = formasPagamentoMap[tPag] || 'outro';
      }

      const det = xmlDoc.getElementsByTagNameNS(ns, 'det')[0];
      const prod = det?.getElementsByTagNameNS(ns, 'prod')[0];
      const descricao = prod ? this.getTagValueNS(prod, ns, 'xProd') : 'Nota Fiscal ' + nNF;

      const existingVendor = this.vendors.find((v) => v.cnpjCpf === vendorCnpj);

      if (existingVendor) {
        this.newObligation.vendorId = existingVendor.id;
      } else {
        this.createVendorFromXml(vendorCnpj, vendorNome);
      }

      this.newObligation.descricao = descricao;
      this.newObligation.valorNominal = valorTotal;
      this.currencyInputValue = this.formatCurrency(valorTotal);
      this.newObligation.numeroNotaFiscal = nNF;
      this.newObligation.serieNotaFiscal = serie;
      this.newObligation.chaveAcessoNFe = chaveNFe;
      this.newObligation.dataEmissao = dataEmissao;
      this.newObligation.dataVencimento = dataVencimento;
      this.newObligation.valorICMS = valorICMS;
      this.newObligation.valorIPI = valorIPI;
      this.newObligation.valorPIS = valorPIS;
      this.newObligation.valorCOFINS = valorCOFINS;
      this.newObligation.valorTotalImpostos = valorICMS + valorIPI + valorPIS + valorCOFINS;
      this.newObligation.formaPagamento = formaPagamento;

      if (formaPagamento) {
        this.newObligation.status = 'pago';
      }

      this.showXmlUpload = false;
      this.errorMessage = '';
    } catch (error) {
      console.error('Erro ao extrair dados do XML:', error);
      this.errorMessage = 'Erro ao processar XML da nota fiscal';
    }
  }

  private getTagValueNS(parent: Element, namespace: string, tagName: string): string {
    const elements = parent.getElementsByTagNameNS(namespace, tagName);
    return elements.length > 0 ? elements[0].textContent || '' : '';
  }

  createVendorFromXml(cnpj: string, nome: string): void {
    const newVendor = {
      tenantId: this.tenantId,
      nome: nome,
      cnpjCpf: cnpj,
      email: '',
      telefone: '',
      banco: '',
      agencia: '',
      contaCorrente: '',
      pix: '',
      ativo: true,
    };

    this.apiService.post('Vendors', newVendor).subscribe({
      next: (response: any) => {
        this.newObligation.vendorId = response.id;
        this.loadSupportingData();
      },
      error: (error) => {
        console.error('Erro ao criar fornecedor:', error);
        this.errorMessage = 'Erro ao cadastrar fornecedor automaticamente';
      },
    });
  }

  onCurrencyInputChange(event: any): void {
    let value = event.target.value;
    value = value.replace(/\D/g, '');
    value = value.replace(/^0+/, '');

    if (value === '') {
      this.currencyInputValue = '';
      this.newObligation.valorNominal = 0;
      return;
    }

    const numericValue = parseFloat(value) / 100;
    this.newObligation.valorNominal = numericValue;

    this.currencyInputValue = new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL',
      minimumFractionDigits: 2,
      maximumFractionDigits: 2,
    }).format(numericValue);
  }

  onCurrencyInputBlur(): void {
    if (this.newObligation.valorNominal === 0) {
      this.currencyInputValue = '';
    }
  }

  onCurrencyInputFocus(): void {
    if (this.newObligation.valorNominal === 0) {
      this.currencyInputValue = '';
    }
  }

  createOrUpdateObligation(): void {
    const obligationData: any = {
      descricao: this.newObligation.descricao,
      valorNominal: this.newObligation.valorNominal,
      dataEmissao: new Date(this.newObligation.dataEmissao + 'T00:00:00').toISOString(),
      dataVencimento: new Date(this.newObligation.dataVencimento + 'T00:00:00').toISOString(),
      numeroNotaFiscal: this.newObligation.numeroNotaFiscal || null,
      serieNotaFiscal: this.newObligation.serieNotaFiscal || null,
      chaveAcessoNFe: this.newObligation.chaveAcessoNFe || null,
      costCenterId: this.newObligation.costCenterId || null,
      expenseCategoryId: this.newObligation.expenseCategoryId || null,
      valorICMS: this.newObligation.valorICMS || 0,
      valorIPI: this.newObligation.valorIPI || 0,
      valorPIS: this.newObligation.valorPIS || 0,
      valorCOFINS: this.newObligation.valorCOFINS || 0,
      status: this.newObligation.status || 'pendente',
      formaPagamento: this.newObligation.formaPagamento || null,
      dataPagamento: this.newObligation.dataPagamento
        ? new Date(this.newObligation.dataPagamento + 'T00:00:00').toISOString()
        : null,
      protocoloPagamento: this.newObligation.protocoloPagamento || null,
    };

    if (this.isEditMode && this.editingObligationId) {
      obligationData.id = this.editingObligationId;
      this.apiService
        .put(`FinancialObligations/${this.editingObligationId}`, obligationData)
        .subscribe({
          next: () => {
            this.closeCreateModal();
            this.loadTenantData();
          },
          error: (error) => {
            console.error('Erro ao editar obrigação:', error);
            this.errorMessage = 'Erro ao editar conta';
          },
        });
    } else {
      obligationData.tenantId = this.tenantId;
      obligationData.vendorId = this.newObligation.vendorId;

      this.apiService.post('FinancialObligations', obligationData).subscribe({
        next: () => {
          this.closeCreateModal();
          this.loadTenantData();
        },
        error: (error) => {
          console.error('Erro ao criar obrigação:', error);
          this.errorMessage = 'Erro ao criar conta';
        },
      });
    }
  }

  formatCurrency(value: number): string {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL',
    }).format(value);
  }

  formatDate(dateString: string): string {
    return new Date(dateString).toLocaleDateString('pt-BR');
  }

  getStatusBadgeClass(status: string): string {
    const statusClasses: { [key: string]: string } = {
      pendente: 'bg-yellow-100 text-yellow-800',
      em_aprovacao: 'bg-blue-100 text-blue-800',
      aprovado: 'bg-green-100 text-green-800',
      aguardando_pagamento: 'bg-purple-100 text-purple-800',
      pago: 'bg-gray-100 text-gray-800',
      cancelado: 'bg-red-100 text-red-800',
      vencido: 'bg-red-100 text-red-800',
    };
    return statusClasses[status] || 'bg-gray-100 text-gray-800';
  }
}
