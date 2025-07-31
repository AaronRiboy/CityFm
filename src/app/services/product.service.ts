import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface ProductViewModel {
  productCode: string;
  description: string;
  basePrice: number;
  markedUpPrice: number;
}

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private apiUrl = 'https://localhost:5040/api/Products';

  constructor(private http: HttpClient) {}

  getProducts(): Observable<ProductViewModel[]> {
    return this.http.get<ProductViewModel[]>(this.apiUrl);
  }
}
