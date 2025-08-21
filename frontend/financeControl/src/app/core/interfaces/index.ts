export interface Account {
  id: string;
  razao_social: string;
  cnpj: string;
  inscricao_estadual?: string;
  nome_fantasia?: string;
  plano_contratado: 'basic' | 'premium' | 'enterprise';
  data_inicio_contrato: Date;
  data_fim_contrato: Date;
}

export interface FinancialObligation {
  id: string;
  descricao: string;
  valorNominal: number;
  dataEmissao: string;
  dataVencimento: string;
  numeroNotaFiscal?: string;
  serieNotaFiscal?: string;
  chaveAcessoNFe?: string;
  status: string;
  tenantId: string;
  vendorId: string;
  vendorNome: string;
  costCenterId?: string;
  costCenterCodigo?: string;
  expenseCategoryId?: string;
  expenseCategoryNome?: string;
  aprovadoPorUserId?: string;
  aprovadoPorUserName?: string;
  dataAprovacao?: string;
  dataPagamento?: string;
  protocoloPagamento?: string;
  dataCriacao: string;
  valorICMS?: number;
  valorIPI?: number;
  formaPagamento?: string;
  valorPIS?: number;
  valorCOFINS?: number;
  valorTotalImpostos?: number;
}

export interface Vendor {
  id: string;
  tenantId: string;
  nome: string;
  cnpjCpf: string;
  email?: string;
  telefone?: string;
  banco?: string;
  agencia?: string;
  contaCorrente?: string;
  pix?: string;
  ativo: boolean;
}

export interface CostCenter {
  id: string;
  tenantId: string;
  codigo: string;
  nome: string;
  descricao?: string;
  ativo: boolean;
}

export interface ExpenseCategory {
  id: string;
  tenantId: string;
  nome: string;
  descricao?: string;
  codigoContaContabil?: string;
  ativo: boolean;
}
export interface User {
  id: string;
  nomeCompleto: string;
  email: string;
  dataCriacao: string;
  accountRole: string;
}

export interface Tenant {
  id: string;
  razaoSocial: string;
  nomeFantasia?: string;
  cnpj: string;
  accountId: string;
  status?: string;
  limiteAprovacaoAutomatica?: number;
  dataCriacao?: string;
  dataAtualizacao?: string;
  ativo?: boolean;
}

export interface UserTenantRole {
  id: string;
  user_id: string;
  tenant_id: string;
  role: 'admin' | 'aprovador_financeiro' | 'visualizador';
}

export interface Fornecedor {
  id: string;
  nome_fantasia: string;
  razao_social: string;
  cnpj?: string;
  cpf?: string;
  email?: string;
  telefone?: string;
  tenant_id: string;
}

export interface Categoria {
  id: string;
  nome: string;
  descricao?: string;
  tenant_id: string;
}

export interface CentroCusto {
  id: string;
  nome: string;
  descricao?: string;
  codigo: string;
  tenant_id: string;
}

export type ObrigacaoStatus =
  | 'pendente'
  | 'em_aprovacao'
  | 'aprovado'
  | 'aguardando_pagamento'
  | 'pago'
  | 'cancelado'
  | 'vencido';

export interface ObrigacaoFinanceira {
  id: string;
  fornecedor_id: string;
  descricao: string;
  numero_documento?: string;
  valor_total: number;
  data_vencimento: Date;
  data_emissao: Date;
  categoria_id: string;
  centro_custo_id: string;
  status: ObrigacaoStatus;
  tenant_id: string;
  fornecedor?: Fornecedor;
  categoria?: Categoria;
  centro_custo?: CentroCusto;
}
export type AgendamentoStatus = 'aguardando' | 'agendado' | 'pago' | 'cancelado';

export type OrdemPagamentoStatus =
  | 'gerada'
  | 'transmitida'
  | 'processada'
  | 'parcialmente_processada'
  | 'falha';

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  user: User;
  tenants: Tenant[];
  current_tenant?: Tenant;
}

export interface CreateAccountPayload {
  account: {
    razaoSocial: string;
    cnpj: string;
    nomeFantasia?: string;
    inscricaoEstadual?: string;
  };
  owner: {
    nomeCompleto: string;
    email: string;
    password: string;
  };
}
