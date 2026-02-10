# ConsoleSimpleMMOGame - ConsoleApp1

`ConsoleApp1`은 **렌더링 없이 논리적으로 동작하는 콘솔 기반 게임 서버 프로토타입**입니다.  
현재 목적은 유니티/언리얼/Win32API 같은 다양한 클라이언트에서 보낼 명령을 서버가 해석하고, 월드(논리 공간)와 오브젝트 상태를 처리할 수 있는 기본 골격을 만드는 것입니다.

---

## 1) 프로젝트가 하는 일

- 콘솔 입력 명령을 큐에 넣고 주기적으로 처리합니다.
- 문자열 명령을 분기/디스패치하여 오브젝트 생성/조회/공간 관리 기능을 수행합니다.
- 통신 명령(JSON 정의)과 게임 액션 명령(JSON 정의)을 파싱하여 패킷 형태로 직렬화할 수 있습니다.
- 접속 인증 정보(`authKey`, endpoint 등)와 사용자 역할/권한 모델의 기본 구조를 제공합니다.

---

## 2) 핵심 구조 (폴더 기준)

### `ConsoleApp1/ConsoleApp1/Application`
게임 로직의 책임을 분리한 애플리케이션 계층입니다.

- `Dispatching/CommandDispatcher.cs`
  - 정확 일치 명령, 접두(prefix) 명령을 등록/실행합니다.
  - 별칭(alias) 등록도 지원합니다.
- `Repositories/GameObjectRepository.cs`
  - 전역 오브젝트 저장소 역할입니다.
  - 이름 기반 조회/집계 기능이 들어 있습니다.
- `Services/VirtualSpaceService.cs`
  - 논리 공간(`VirtualSpace`)의 생성/삭제/조회/전체 링크 정리를 담당합니다.

### `ConsoleApp1/ConsoleApp1/Protocol`
명령 프로토콜 처리 계층입니다.

- `Providers/JsonCommandDefinitionProvider.cs`
  - JSON 정의 파일을 로드합니다.
- `Parsing/CommandParser.cs`
  - 입력 문자열을 `CommandSet`으로 파싱/검증합니다.
- `Serialization/PipePacketSerializer.cs`
  - `PREFIX|arg1|arg2|payload` 형태로 패킷 문자열을 만듭니다.
- `CommandDefinitions.cs`
  - 프로토콜/게임액션 정의 모델 클래스가 있습니다.

### `ConsoleApp1/ConsoleApp1/Runtime`
실행 루프 계층입니다.

- `ConsoleInputLoop.cs`: 콘솔 입력을 큐에 적재
- `CommandQueueProcessor.cs`: 큐의 명령을 `GameProgram`에 전달
- `NetworkLoop.cs`: 주기적으로 큐 처리 실행

### `ConsoleApp1/ConsoleApp1/Spaces`
렌더링 없는 논리 공간 계층입니다.

- `VirtualSpace.cs`
  - 오브젝트 링크/중복방지/링크 해제/이름별 집계를 제공합니다.

### 기타 주요 파일

- `GameProgram.cs`
  - 각 컴포넌트를 조합하고 명령 등록 테이블을 구성합니다.
- `CommandFactory.cs`
  - 프로토콜 명령/게임 액션 명령 파싱 실행 진입점입니다.
- `ConnectionAuthenticator.cs`, `ConnectedUser.cs`
  - 접속 인증/사용자 역할/권한 모델의 기본 기능입니다.

---

## 3) 현재 제공 명령 (GameProgram)

### 오브젝트 관련

- `스피키`
- `스핔이`
- `버터`
- `한정현` (`대뾴니`, `대뾰니` 별칭)
- `개체 개수 표시`
- `찾기 [이름]`
- `모두찾기`
- `주말농장` (오브젝트 + 공간 링크 초기화)

### 공간(VirtualSpace) 관련

- `공간생성 [공간이름]`
- `공간삭제 [공간이름]`
- `공간에넣기 [공간명] [오브젝트명]`
- `공간목록`
- `공간조회 [공간이름]`

> 하위호환으로 기존 `세계생성/세계삭제/세계에넣기/세계목록/세계조회`도 같은 동작에 연결되어 있습니다.

### 기타

- `/hostalarm [메시지]`

---

## 4) JSON 기반 명령 정의

### 4-1. 통신 프로토콜 명령

파일: `ConsoleApp1/ConsoleApp1/Commands/CommandFactoryDecodeDefinition.json`

구조:

```json
{
  "commandName": {
    "prefix": "REQ",
    "params": ["route"],
    "payloadRequired": false,
    "payloadOptional": true
  }
}
```

### 4-2. 게임 액션 명령

파일: `ConsoleApp1/ConsoleApp1/Commands/GameActionDefinition.json`

구조:

```json
{
  "terrain.create": {
    "category": "world",
    "prefix": "ACT_TERRAIN_CREATE",
    "params": ["worldId", "terrainType", "x", "y", "z"],
    "payloadRequired": false,
    "payloadOptional": true,
    "description": "설명"
  }
}
```

---

## 5) 접속/권한 모델 요약

- `ConnectionAuthenticator`
  - 접속 시 `authKey`를 발급하고 endpoint(IP/MAC)와 연결 정보를 저장합니다.
  - 인증 여부(`IsAuthorized`)와 연결 조회/해제를 지원합니다.
- `ConnectedUser`
  - `UserRole` 기반 권한 체크(`HasPermission`) 기능이 있습니다.
  - 로컬 접속은 기본적으로 높은 권한(`LocalAdmin`)이 부여됩니다.

---

## 6) 실행/개발 참고

- 현재 저장소에는 솔루션/프로젝트 메타 파일이 포함되어 있지 않아, 환경에 따라 바로 `dotnet build`가 되지 않을 수 있습니다.
- 이 프로젝트는 기능/아키텍처 프로토타입 성격이 강하며, 네트워크/DB/인증 고도화는 다음 단계 대상입니다.

---

## 7) SOLID가 아직 익숙하지 않아도 괜찮습니다

취준 과정에서 가장 중요한 건 “원칙 암기”보다 **코드가 왜 분리되어야 하는지 체감**하는 것입니다.

현재 구조는 아래처럼 읽으면 쉽습니다.

- 입력/루프는 `Runtime`
- 업무 규칙(오브젝트/공간)은 `Application`
- 통신 규칙은 `Protocol`
- 공간 모델은 `Spaces`

즉, **문제가 생기면 어디를 열어야 하는지 예측 가능한 구조**를 만드는 것이 1차 목표입니다.

화이팅입니다. 🙌
