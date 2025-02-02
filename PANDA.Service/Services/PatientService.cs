using PANDA.ClientModel.Model.Patient;
using PANDA.Repository.Model;
using PANDA.Repository.Repositories.Interfaces;
using PANDA.Service.Exceptions;
using PANDA.Service.Services.Interfaces;
using PANDA.Service.Validation;

namespace PANDA.Service.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<GetPatientResponse> GetPatient(string patientIdentifier, CancellationToken cancellationToken)
        {
            await ThrowIfPatientDoesNotExist(patientIdentifier, cancellationToken);

            Patient patient = await _patientRepository.GetPatientAsync(patientIdentifier, cancellationToken);

            return new GetPatientResponse
            {
                Id = patient.Id,
                LocalIdentifier = patient.LocalIdentifier,
                NhsNumber = patient.NhsNumber,
                Forename = patient.Forename,
                Surname = patient.Surname,
                DateOfBirth = patient.DateOfBirth,
                AddressLine1 = patient.Address.AddressLine1,
                AddressLine2 = patient.Address.AddressLine2,
                AddressLine3 = patient.Address.AddressLine3,
                AddressLine4 = patient.Address.AddressLine4,
                AddressLine5 = patient.Address.AddressLine5,
                Postcode = patient.Address.PostCode
            };
        }

        public async Task<CreatePatientResponse> CreatePatient(CreatePatientRequest createPatientRequest, CancellationToken cancellationToken)
        {
            await ThrowIfPatientExists(createPatientRequest.LocalIdentifier, cancellationToken);
            await ThrowIfPatientIsDeleted(createPatientRequest.LocalIdentifier, cancellationToken);
            ThrowIfNhsNumberFailsChecksumValidation(createPatientRequest.NhsNumber);

            Patient patient = new Patient()
            {
                CreatedDateTime = DateTime.UtcNow,
                UpdatedDateTime = DateTime.UtcNow,
                Forename = createPatientRequest.Forename,
                Surname = createPatientRequest.Surname,
                LocalIdentifier = createPatientRequest.LocalIdentifier,
                NhsNumber = createPatientRequest.NhsNumber,
                DateOfBirth = createPatientRequest.DateOfBirth,
                Address = new Address
                {
                    AddressLine1 = createPatientRequest.AddressLine1,
                    AddressLine2 = createPatientRequest.AddressLine2,
                    AddressLine3 = createPatientRequest.AddressLine3,
                    AddressLine4 = createPatientRequest.AddressLine4,
                    AddressLine5 = createPatientRequest.AddressLine5,
                    PostCode = createPatientRequest.PostCode
                }
            };

            await _patientRepository.AddAsync(patient, cancellationToken);

            return new CreatePatientResponse
            {
                Id = patient.Id,
                LocalIdentifier = patient.LocalIdentifier,
                NhsNumber = patient.NhsNumber,
                DateOfBirth = patient.DateOfBirth,
                Forename = patient.Forename,
                Surname = patient.Surname,
                AddressLine1 = patient.Address.AddressLine1,
                AddressLine2 = patient.Address.AddressLine2,
                AddressLine3 = patient.Address.AddressLine3,
                AddressLine4 = patient.Address.AddressLine4,
                AddressLine5 = patient.Address.AddressLine5,
                Postcode = patient.Address.PostCode
            };
        }

        public async Task<UpdatePatientResponse> UpdatePatient(int patientId, UpdatePatientRequest updatePatientRequest, CancellationToken cancellationToken)
        {
            await ThrowIfPatientDoesNotExist(patientId, cancellationToken);
            await ThrowIfPatientIsDeleted(patientId, cancellationToken);
            ThrowIfNhsNumberFailsChecksumValidation(updatePatientRequest.NhsNumber);

            Patient patient = await _patientRepository.GetPatientAsync(patientId, cancellationToken);

            patient.NhsNumber = updatePatientRequest.NhsNumber;
            patient.Forename = updatePatientRequest.Forename;
            patient.Surname = updatePatientRequest.Surname;
            patient.DateOfBirth = updatePatientRequest.DateOfBirth;
            patient.Address.AddressLine1 = updatePatientRequest.AddressLine1;
            patient.Address.AddressLine2 = updatePatientRequest.AddressLine2;
            patient.Address.AddressLine3 = updatePatientRequest.AddressLine3;
            patient.Address.AddressLine4 = updatePatientRequest.AddressLine4;
            patient.Address.AddressLine5 = updatePatientRequest.AddressLine5;
            patient.Address.PostCode = updatePatientRequest.PostCode;

            await _patientRepository.SaveChangesAsync(cancellationToken);

            return new UpdatePatientResponse
            {
                Id = patient.Id,
                LocalIdentifier = patient.LocalIdentifier,
                NhsNumber = patient.NhsNumber,
                Forename = patient.Forename,
                Surname = patient.Surname,
                DateOfBirth = patient.DateOfBirth,
                AddressLine1 = patient.Address.AddressLine1,
                AddressLine2 = patient.Address.AddressLine2,
                AddressLine3 = patient.Address.AddressLine3,
                AddressLine4 = patient.Address.AddressLine4,
                AddressLine5 = patient.Address.AddressLine5,
                Postcode = patient.Address.PostCode
            };

        }

        public async Task DeletePatient(int patientId, CancellationToken cancellationToken)
        {
            if (!await _patientRepository.IsExistingPatientAsync(patientId, cancellationToken))
            {
                throw new HandledException($"Patient identifier {patientId} does not exist", 404);
            }

            await _patientRepository.DeletePatientAsync(patientId, cancellationToken);
        }

        private async Task ThrowIfPatientDoesNotExist(string localIdentifier, CancellationToken cancellationToken)
        {
            if (!await _patientRepository.IsExistingPatientAsync(localIdentifier, cancellationToken))
            {
                throw new HandledException($"Patient with identifier '{localIdentifier}' does not exist", 404);
            }
        }

        private async Task ThrowIfPatientDoesNotExist(int id, CancellationToken cancellationToken)
        {
            if (!await _patientRepository.IsExistingPatientAsync(id, cancellationToken))
            {
                throw new HandledException($"Patient with identifier '{id}' does not exist", 404);
            }
        }

        private async Task ThrowIfPatientIsDeleted(string localIdentifier, CancellationToken cancellationToken)
        {
            if (await _patientRepository.IsDeletedPatientAsync(localIdentifier, cancellationToken))
            {
                throw new HandledException($"Patient with identifier '{localIdentifier}' is marked as deleted", 400);
            }
        }

        private async Task ThrowIfPatientExists(string localIdentifier, CancellationToken cancellationToken)
        {
            if (await _patientRepository.IsExistingPatientAsync(localIdentifier, cancellationToken))
            {
                throw new HandledException($"Patient identifier {localIdentifier} already exist", 400);
            }
        }

        private async Task ThrowIfPatientIsDeleted(int id, CancellationToken cancellationToken)
        {
            if (await _patientRepository.IsDeletedPatientAsync(id, cancellationToken))
            {
                throw new HandledException($"Patient with identifier '{id}' is marked as deleted", 400);
            }
        }

        private void ThrowIfNhsNumberFailsChecksumValidation(string nhsNumber)
        {
            if (!NhsNumberValidator.IsValidNHSNumber(nhsNumber))
            {
                throw new HandledException($"NHS Number {nhsNumber} is not valid ", 400);
            }
        }
    }
}
