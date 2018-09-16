import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';   

import { BalanceService } from '../../../services/balance.service';
import { Balance } from '../../../models/balance.model'

@Component({
  selector: 'app-balance-list',
  templateUrl: './balance-list.component.html',
  styleUrls: ['./balance-list.component.css']
})
export class BalanceListComponent implements OnInit {

  constructor(public balanceService : BalanceService, private toastr : ToastrService) { }

  ngOnInit() {
  }

  showForEdit(balance: Balance) {
    this.balanceService.selectedBalance = Object.assign({}, balance);;
  }

  onDelete(id: string) {
    if (confirm('Are you sure to delete this record ?') == true) {
      this.balanceService.deleteBalance(id)
      .subscribe(x => {

        let dateFilter = this.balanceService.effectiveDatesList.length > 0 ? this.balanceService.effectiveDatesList[0] : null;

        this.balanceService.getBalanceList(dateFilter);
        
        this.toastr.warning("Deleted Successfully", "Balance");
      })
    }
  }

}
