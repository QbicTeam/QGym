import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'dataFilter'
})
export class FilterPipe implements PipeTransform {

  transform(data: any[], searchText: string): any[] {

    console.log(data);

    return data;
  }

}
