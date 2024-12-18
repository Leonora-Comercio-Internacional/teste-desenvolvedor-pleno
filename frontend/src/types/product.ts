export interface Product {
  id: number;
  name: string | null;
  price: number;
  description: string | null;
  isDeleted: boolean;
  categoryId: number;
  supplierId: Array<number>;
  dateAdded: string;
}

export interface ProductRequest {
  name: string | null;
  price: number;
  description: string | null;
  categoryId: number;
  supplierId: Array<number>;
}