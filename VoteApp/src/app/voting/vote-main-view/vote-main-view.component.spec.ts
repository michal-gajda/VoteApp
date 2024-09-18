import { TestBed } from '@angular/core/testing';
import { VoteMainViewComponent } from './vote-main-view.component';
import { AppModule } from 'src/app/app.module';

describe('VoteMainView', () => {

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [AppModule]
    }).compileComponents();
  });

  it('should be created', () => {
    const fixture = TestBed.createComponent(VoteMainViewComponent);
    const comp = fixture.componentInstance;
    expect(comp).toBeTruthy();
  });
});
