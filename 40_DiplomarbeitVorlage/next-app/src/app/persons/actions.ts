/**
 * @file Serveractions für die Speicherung von Personen.
 */
"use server"
import { PrismaClient } from '@prisma/client'
import { createValidationError, PersonValidations, ValidationError } from '../utils/validations';
import { Person } from '.prisma/client';

const prisma = new PrismaClient()

export async function getPersons() {
  // Den internen id Wert wollen wir nicht preisgeben, deswegen selektieren wir nur felder, die wir anzeigen wollen.
  // Das ist der Grund, warum wir eine GUID zusätzlich zum ID Wert haben.
  return await prisma.person.findMany({
    select: { guid: true, firstname: true, lastname: true, birthDate: true }
  });
}

export async function addPerson(formData: FormData): Promise<ValidationError<Person> | Person> {
  const data = {
    firstname: formData.get("firstname") as string,
    lastname: formData.get("lastname") as string,
    birthDate: formData.get("birthDate") ? new Date(formData.get("birthDate") as string) : undefined
  }
  // Validierung wird in utils/validations.ts definiert.
  const validation = PersonValidations.safeParse(data);
  if (!validation.success) {
    return createValidationError(validation.error);
  }
  try {
    return await prisma.person.create({ data: data });
  }
  catch (e) {
    return createValidationError(e);
  }
}