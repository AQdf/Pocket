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
    this.balanceService.getBalanceList();
  }

  showForEdit(balance: Balance) {
    this.balanceService.selectedBalance = Object.assign({}, balance);;
  }

  onDelete(id: string) {
    if (confirm('Are you sure to delete this record ?') == true) {
      this.balanceService.deleteBalance(id)
      .subscribe(x => {
        this.balanceService.getBalanceList();
        this.toastr.warning("Deleted Successfully", "Balance");
      })
    }
  }

}
