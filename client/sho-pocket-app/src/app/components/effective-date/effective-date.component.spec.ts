import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EffectiveDateComponent } from './effective-date.component';

describe('EffectiveDateComponent', () => {
  let component: EffectiveDateComponent;
  let fixture: ComponentFixture<EffectiveDateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EffectiveDateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EffectiveDateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
