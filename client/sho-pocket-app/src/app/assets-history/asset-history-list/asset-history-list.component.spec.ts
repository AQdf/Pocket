import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssetHistoryListComponent } from './asset-history-list.component';

describe('AssetHistoryListComponent', () => {
  let component: AssetHistoryListComponent;
  let fixture: ComponentFixture<AssetHistoryListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssetHistoryListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssetHistoryListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
