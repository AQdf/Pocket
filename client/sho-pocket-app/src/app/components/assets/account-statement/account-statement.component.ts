import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { BankSyncService } from '../../../services/bank-sync.service';
import { AccountTransaction } from '../../../models/account-transaction.model';

@Component({
  selector: 'app-account-statement',
  templateUrl: './account-statement.component.html',
  styleUrls: ['./account-statement.component.css']
})
export class AccountStatementComponent implements OnInit {

  id: string = null;
  transactions: AccountTransaction[] = [];

  constructor(private bankSyncService : BankSyncService, private activeRouter : ActivatedRoute) { }

  ngOnInit() {
    this.id = this.activeRouter.snapshot.params['id'];
  }

  loadTransactions() {
    this.bankSyncService.loadTransactions(this.id).subscribe((response: AccountTransaction[]) => {
      this.transactions = response;
    });
  }
}
