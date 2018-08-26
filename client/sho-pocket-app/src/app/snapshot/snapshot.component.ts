import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';   

import { SnapshotService } from './snapshot.service';

@Component({
  selector: 'app-snapshot',
  templateUrl: './snapshot.component.html',
  styleUrls: ['./snapshot.component.css']
})
export class SnapshotComponent implements OnInit {

  snapshots: string[];

  constructor(public snapshotService : SnapshotService, private toastr : ToastrService) { }

  ngOnInit() {
    this.snapshots = ["28 Aug 2018", "28 Jul 2018", "28 Jun 2018"] 
  }

  takeSnapshot() {
    this.snapshotService.takeSnapshot()
    .subscribe(isSaved => {
      if(isSaved) {
        this.toastr.success('Snapshot Successfully Added', 'Snapshot');
      } else {
        this.toastr.success('Snapshot of this day already exists', 'Snapshot');
      }
    });
  }
}
