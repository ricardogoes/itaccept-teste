import { Component, OnInit } from '@angular/core';
import { delay, Observable } from 'rxjs';
import { LoadingService } from './loading.service';

@Component({
    selector: 'itaccept-loading',
    templateUrl: './loading.component.html',
    styleUrls: ['./loading.component.css']
})
export class LoadingComponent implements OnInit {

    loading$: Observable<boolean>;

	  constructor(
		    public loadingService: LoadingService
	  ) {}

    ngOnInit(): void {
          this.listenToLoading();
    }

    listenToLoading(): void {
        this.loading$ = this.loadingService.loading.pipe(delay(0)); // This prevents a ExpressionChangedAfterItHasBeenCheckedError for subsequent requests
    }

}
