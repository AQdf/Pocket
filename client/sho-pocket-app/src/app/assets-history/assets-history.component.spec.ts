import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssetsHistoryComponent } from './assets-history.component';

describe('AssetsHistoryComponent', () => {
  let component: AssetsHistoryComponent;
  let fixture: ComponentFixture<AssetsHistoryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssetsHistoryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssetsHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
