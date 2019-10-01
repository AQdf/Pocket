import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BalanceNoteComponent } from './balance-note.component';

describe('BalanceNoteComponent', () => {
  let component: BalanceNoteComponent;
  let fixture: ComponentFixture<BalanceNoteComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BalanceNoteComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BalanceNoteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
