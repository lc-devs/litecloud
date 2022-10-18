document.write(
 `
<div class="modal fade" id="btnSuccessEntryModal" tabindex="-1" role="dialog"	aria-labelledby="btnSuccessEntryModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-md" role="document">
        <div class="modal-content">
            <div class="modal-body">
                <div class="modal-header">
                    <button type="button" class="close" hidden data-dismiss="modal" aria-label="Close"
                        id="btnCloseSuccessAlert">
                    <span aria-hidden="true">×</span>
                    </button>
                </div>
                <div class="d-flex my-3 align-items-center">
                    <div class="col-3">
                        <i class="fas fa-check-circle fa-4x text-success"></i>
                    </div>
                    <p id="successMessage" class="px-2">Entry has been 
                        <span id="successServiceMethod"
                            class="text-success">
                        successfully
                        </span>
                    </p>
                </div>
                <div class=" row d-flex justify-content-between text-right p-3">
                    <div>
                    </div>
                    <div class="col-6 mt-2">
                        <button type="button" class="btn btn-primary col-5" id="btnCloseSuccessEntry">
                        OK
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<div class="modal fade" id="btnFailedModal" tabindex="-1" role="dialog" aria-labelledby="btnFailedModalLabel" data-backdrop="static">
	<div class="modal-dialog modal-md" role="document">
		<div class="modal-content">
			<div class="modal-body">
				<div class="modal-header">
					<button type="button" hidden class="close" data-dismiss="modal" aria-label="Close"
						id="btnCloseFailedEntry">
					<span aria-hidden="true">×</span>
					</button>
				</div>
				<div class="d-flex my-3 align-items-center">
					<div class="col-3">
						<i class="fas fa-times-circle fa-4x text-danger"></i>
					</div>
					<p id="failedMessage" class="px-2">Failed to <span class="text-danger" id="failedMethod"></span>
						record. Please
						contact
						administrator
					</p>
				</div>
				<div class=" row d-flex justify-content-between text-right p-3">
					<div>
					</div>
					<div class="col-6 mt-5">
						<button type="button" class="btn btn-primary col-5" id="btnCloseFailedAlert">
						OK
						</button>
					</div>
				</div>
			</div>
		</div>
	</div>
</div>
<div class="modal fade" id="firstconfirm" tabindex="-1" role="dialog" aria-labelledby="firstconfirmLabel" aria-hidden="true" data-backdrop="static">
	<div class="modal-dialog modal-sm" role="document">
		<div class="modal-content">
			<div class="modal-header">
				
				<button type="button" class="close" data-dismiss="modal" aria-label="Close">
				</button>
			</div>
			<div class="modal-body">
				<p id="message">
					Are you sure you want to save this?
				</p>
			</div>
			<div class="modal-footer">
				<button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
				<button type="button" class="btn btn-primary" data-dismiss="modal" data-toggle="modal" data-target="#secondconfirm">Yes</button>
			</div>
		</div>
	</div>
</div>
<div class="modal fade" id="secondconfirm" tabindex="-1" role="dialog" aria-labelledby="secondconfirmLabel" aria-hidden="true" data-backdrop="static">
	<div class="modal-dialog modal-sm" role="document">
		<div class="modal-content">
			<div class="modal-header">
				
				<button type="button" class="close" data-dismiss="modal" aria-label="Close"
					id="btnCloseConfimrTwo">
					<span aria-hidden="true">×</span>
				</button>
			</div>
			<div class="modal-body">
				<p>
					Click OK to Confirm.
				</p>
			</div>
			<div class="modal-footer">
				<button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
				<button type="button" class="btn btn-primary" data-dismiss="modal" data-toggle="modal" id="btnExecute">Confirm</button>
			</div>
		</div>
	</div>
</div>
`);