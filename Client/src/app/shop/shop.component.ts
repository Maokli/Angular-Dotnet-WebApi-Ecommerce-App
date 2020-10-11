import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ShopService } from './shop.service';
import { IProduct } from '../shared/models/product';
import { IBrand } from '../shared/models/brands';
import { IType } from '../shared/models/productType';
import { ShopParams } from '../shared/models/shopParams';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss'],
})
export class ShopComponent implements OnInit {
  @ViewChild('search',{static:true}) searchTerm: ElementRef;
  products: IProduct[];
  brands: IBrand[];
  types: IType[];
  shopParams = new ShopParams();
  totalCount:number;
  sortOptions = [{name:"Alphabetical", value:"name"},
                 {name:"Price: Low to High", value:"priceAsc"},
                 {name:"Price: High to Low", value:"priceDesc"}
];
  constructor(private shopservice: ShopService) {}

  ngOnInit(): void {
    this.GetProducts();
    this.GetBrands();
    this.GetTypes();
  }
  GetProducts() {
    this.shopservice
      .GetProducts(this.shopParams)
      .subscribe(
        (response) => {
          this.products = response.data;
          this.shopParams.pageNumber= response.pageIndex;
          this.shopParams.pageSize = response.pageSize;
          this.totalCount = response.count;
        },
        (error) => {
          console.log(error);
        }
      );
  }
  GetBrands() {
    this.shopservice.GetBrands().subscribe(
      (response) => {
        this.brands = [{ id: 0, name: 'All' }, ...response];
      },
      (error) => {
        console.log(error);
      }
    );
  }
  GetTypes() {
    this.shopservice.GetTypes().subscribe(
      (response) => {
        this.types = [{ id: 0, name: 'All' }, ...response];
      },
      (error) => {
        console.log(error);
      }
    );
  }
  OnBrandSelected(brandId: number) {
    this.shopParams.pageNumber = 1;
    this.shopParams.brandId = brandId;
    this.GetProducts();
  }
  OnTypeSelected(typeId: number) {
    this.shopParams.pageNumber = 1;
    this.shopParams.typeId = typeId;
    this.GetProducts();
  }
  OnSortSelected(sort:string){
    this.shopParams.sort = sort;
    this.GetProducts();
  }
  OnPageChanged(event:any){
    if(this.shopParams.pageNumber !== event.page){
      this.shopParams.pageNumber = event.page;
      this.GetProducts();
    }
  }
  OnSearch(){
    this.shopParams.pageNumber = 1;
    this.shopParams.search = this.searchTerm.nativeElement.value;
    this.GetProducts();
  }
  OnReset(){
    this.searchTerm.nativeElement.value = "";
    this.shopParams = new ShopParams();
    this.GetProducts();
  }
}
