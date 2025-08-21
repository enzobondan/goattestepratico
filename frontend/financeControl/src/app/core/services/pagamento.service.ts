import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { AgendamentoPagamento, OrdemPagamento, TransacaoPagamento } from '../interfaces';

@Injectable({
  providedIn: 'root',
})
export class PagamentoService {
  constructor(private api: ApiService) {}

  getAgendamentos(): Observable<AgendamentoPagamento[]> {
    return this.api.get<AgendamentoPagamento[]>('agendamentos-pagamento');
  }

  getAgendamentoById(id: string): Observable<AgendamentoPagamento> {
    return this.api.get<AgendamentoPagamento>(`agendamentos-pagamento/${id}`);
  }

  updateAgendamento(
    id: string,
    agendamento: Partial<AgendamentoPagamento>
  ): Observable<AgendamentoPagamento> {
    return this.api.put<AgendamentoPagamento>(`agendamentos-pagamento/${id}`, agendamento);
  }

  getOrdensPagamento(): Observable<OrdemPagamento[]> {
    return this.api.get<OrdemPagamento[]>('ordens-pagamento');
  }

  getOrdemPagamentoById(id: string): Observable<OrdemPagamento> {
    return this.api.get<OrdemPagamento>(`ordens-pagamento/${id}`);
  }

  createOrdemPagamento(agendamentosIds: string[]): Observable<OrdemPagamento> {
    return this.api.post<OrdemPagamento>('ordens-pagamento', { agendamentos_ids: agendamentosIds });
  }

  transmitOrdemPagamento(id: string): Observable<OrdemPagamento> {
    return this.api.post<OrdemPagamento>(`ordens-pagamento/${id}/transmit`, {});
  }

  getTransacoesByOrdem(id: string): Observable<TransacaoPagamento[]> {
    return this.api.get<TransacaoPagamento[]>(`ordens-pagamento/${id}/transacoes`);
  }

  processRetornoBanco(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    return this.api.upload<any>('pagamentos/process-retorno', formData);
  }
}
