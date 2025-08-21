import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { ExpenseCategory } from '../interfaces';

@Injectable({
  providedIn: 'root',
})
export class ExpenseCategoryService {
  constructor(private api: ApiService) {}

  getExpenseCategoriesByTenant(tenantId: string): Observable<ExpenseCategory[]> {
    return this.api.get<ExpenseCategory[]>(`ExpenseCategories/tenant/${tenantId}`);
  }

  getExpenseCategoryById(id: string): Observable<ExpenseCategory> {
    return this.api.get<ExpenseCategory>(`ExpenseCategories/${id}`);
  }

  createExpenseCategory(category: Partial<ExpenseCategory>): Observable<ExpenseCategory> {
    return this.api.post<ExpenseCategory>('ExpenseCategories', category);
  }

  updateExpenseCategory(
    id: string,
    category: Partial<ExpenseCategory>
  ): Observable<ExpenseCategory> {
    return this.api.put<ExpenseCategory>(`ExpenseCategories/${id}`, category);
  }

  deleteExpenseCategory(id: string): Observable<void> {
    return this.api.delete<void>(`ExpenseCategories/${id}`);
  }
}
