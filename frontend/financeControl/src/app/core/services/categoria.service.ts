import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { Categoria, CentroCusto } from '../interfaces';

@Injectable({
  providedIn: 'root',
})
export class CategoriaService {
  constructor(private api: ApiService) {}

  getCategorias(): Observable<Categoria[]> {
    return this.api.get<Categoria[]>('categorias');
  }

  getCategoriaById(id: string): Observable<Categoria> {
    return this.api.get<Categoria>(`categorias/${id}`);
  }

  createCategoria(categoria: Partial<Categoria>): Observable<Categoria> {
    return this.api.post<Categoria>('categorias', categoria);
  }

  updateCategoria(id: string, categoria: Partial<Categoria>): Observable<Categoria> {
    return this.api.put<Categoria>(`categorias/${id}`, categoria);
  }

  deleteCategoria(id: string): Observable<void> {
    return this.api.delete<void>(`categorias/${id}`);
  }

  getCentrosCusto(): Observable<CentroCusto[]> {
    return this.api.get<CentroCusto[]>('centros-custo');
  }

  getCentroCustoById(id: string): Observable<CentroCusto> {
    return this.api.get<CentroCusto>(`centros-custo/${id}`);
  }

  createCentroCusto(centroCusto: Partial<CentroCusto>): Observable<CentroCusto> {
    return this.api.post<CentroCusto>('centros-custo', centroCusto);
  }

  updateCentroCusto(id: string, centroCusto: Partial<CentroCusto>): Observable<CentroCusto> {
    return this.api.put<CentroCusto>(`centros-custo/${id}`, centroCusto);
  }

  deleteCentroCusto(id: string): Observable<void> {
    return this.api.delete<void>(`centros-custo/${id}`);
  }
}
