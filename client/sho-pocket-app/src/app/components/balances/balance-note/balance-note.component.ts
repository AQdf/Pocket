import { Component, OnInit, Input, EventEmitter, OnChanges, SimpleChange, Output } from '@angular/core';
import { NgForm } from '@angular/forms'
import { ToastrService } from 'ngx-toastr';
import { BalancesNoteService } from '../../../services/balances-note.service';
import { BalanceNote } from '../../../models/balance-note.model';

@Component({
  selector: 'app-balance-note',
  templateUrl: './balance-note.component.html',
  styleUrls: ['./balance-note.component.css']
})
export class BalanceNoteComponent implements OnInit {

  constructor(private balanceNoteService: BalancesNoteService, private toastr: ToastrService) { }

  @Input() effectiveDate: string;
  @Output() shouldReload = new EventEmitter<boolean>();
 
  reloadEffectiveDates(shouldReload: boolean) {
    this.shouldReload.emit(shouldReload);
  }

  note: string;

  ngOnInit() {
  }

  ngOnChanges(changes: {[propKey: string]: SimpleChange}) {
    this.effectiveDate = changes.effectiveDate.currentValue;
    if (this.effectiveDate) {
      this.reloadBalanceNote();
    }
  }

  reloadBalanceNote() {
    this.balanceNoteService.getNote(this.effectiveDate).subscribe((balanceNote: BalanceNote) => {
      this.note = balanceNote && balanceNote.content;
    });
  }

  saveNote() {
    let balanceNote: BalanceNote = {
      effectiveDate: this.effectiveDate,
      content: this.note
    };

    this.balanceNoteService.patchNote(balanceNote).subscribe((balanceNote: BalanceNote) => {
      this.note = balanceNote.content;
      this.toastr.success('Saved Successfully!', 'Note');
    });
  }
}
