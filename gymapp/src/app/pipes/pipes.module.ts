import { NgModule } from '@angular/core';
import { FilterPipe } from './filter.pipe';

@NgModule({
  imports: [ FilterPipe ],
  exports: [ FilterPipe ]
})
export class PipesModule { }
